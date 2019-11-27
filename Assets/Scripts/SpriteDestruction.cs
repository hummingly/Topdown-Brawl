using Delaunay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteDestruction : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private PolygonCollider2D sourcePolyCollider;

    public int splitPoints;

    List<GameObject> pieces = new List<GameObject>();

    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        sourcePolyCollider = GetComponent<PolygonCollider2D>();

        //CopyInitialSprite(0,1,2);
        genPieces(splitPoints);

        //Destroy(gameObject);
        spriteRend.enabled = false;
    }

 
    private void genPieces(int splitPoints)
    {
        List<Vector2> orgPoints = new List<Vector2>(sourcePolyCollider.GetPath(0));
        List<Vector2> points = new List<Vector2>(orgPoints);

        Rect bounds = new Rect(spriteRend.bounds.center - transform.position - spriteRend.bounds.extents, spriteRend.bounds.size);

        // Prepare more points to cut
        for (int i = 0; i < splitPoints; i++)
            points.Add(bounds.center + new Vector2(Random.Range(0, bounds.width) - bounds.width/2, Random.Range(0, bounds.height) - bounds.height / 2));
        


        // Create actual voronoi logic (cut up plane)
        Voronoi voronoi = new Voronoi(points, null, bounds);

        foreach (List<Vector2> region in voronoi.Regions())
        {
            // Take voronoi noise "plane" and fit it to the org sprite
            List<List<Vector2>> clipped = ClipperHelper.clip(orgPoints, region);

            foreach (List<Vector2> clipedRegion in clipped)
                genPiece(clipedRegion);
        }
    }

    private void genPiece(List<Vector2> points)
    {
        //Vector3 orgPos = transform.localPosition;
        Vector3 orgScale = transform.localScale;
        Quaternion orgRot = transform.localRotation;


        GameObject newGO = new GameObject("Test Mesh");
        pieces.Add(newGO);
        newGO.transform.position = transform.position;
        newGO.transform.rotation = transform.rotation;
        newGO.transform.localScale = transform.localScale;

        var mf = newGO.AddComponent<MeshFilter>();
        var mr = newGO.AddComponent<MeshRenderer>();

        mr.material = GetMat(spriteRend);

        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        /*
        // TODO: make for n vertices?
        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[3];

        vertices[0] = new Vector3(points[0].x, points[0].y, 0);
        vertices[1] = new Vector3(points[1].x, points[1].y, 0);
        vertices[2] = new Vector3(points[2].x, points[2].y, 0);
        triangles[0] = 2;
        triangles[1] = 1;
        triangles[2] = 0;

        mesh.vertices = vertices;
        mesh.triangles = triangles;*/

        /*mesh.vertices = System.Array.ConvertAll(spriteRend.sprite.vertices, i => (Vector3)i);
        mesh.triangles = System.Array.ConvertAll(spriteRend.sprite.triangles, i => (int)i);*/
        //mesh.uv = sprite.uv; //mf.mesh.uv = calcUV(vertices, spriteRend, transform); // TODO: if there is a texture also use uv


        Voronoi voronoi = new Voronoi(points, null, new Rect(mesh.bounds.center, mesh.bounds.size));

        Vector3[] vertices = getVertices(voronoi);
        int[] triangles = getTriangles(voronoi);
        List<Vector2> colliderPoints = new List<Vector2>(points);

        // Center the pieces (without this they can be correct, but then their mesh pivot is wrong, so all of them have the same position)
        Vector3 totalPieceVerts = Vector3.zero;
        for (int i = 0; i < vertices.Length; i++)
            totalPieceVerts += vertices[i];
        Vector2 pieceCenter = totalPieceVerts / vertices.Length;
        Vector2 pieceOffset = (Vector2)newGO.transform.InverseTransformPoint(newGO.transform.position) - pieceCenter;

        for (int i = 0; i < vertices.Length; i++)
            vertices[i] += (Vector3)pieceOffset;

        for (int i = 0; i < colliderPoints.Count; i++)
            colliderPoints[i] += pieceOffset;


        //TODO: pieces still not perfectly centered?
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        var coll = newGO.AddComponent<PolygonCollider2D>();
        coll.SetPath(0, colliderPoints);


        //for (int i = 0; i < vertices.Length; i++)
        //    mesh.vertices[i] += (Vector3)pieceOffset;
        Vector2 pivot = newGO.transform.InverseTransformPoint(newGO.transform.position);
        newGO.transform.localPosition = newGO.transform.TransformPoint(pivot - pieceOffset);


        newGO.transform.localRotation = orgRot;
        newGO.transform.localScale = orgScale;

        newGO.transform.parent = transform;


        //DestructibleBlock dest = newGO.AddComponent<DestructibleBlock>();
        newGO.tag = "Destruction Piece";

        Rigidbody2D rb = newGO.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        rb.isKinematic = true;
    }



    private Vector3[] getVertices(Voronoi voronoi)
    {
        var test = voronoi.SiteCoords();
        Vector3[] vertices = new Vector3[test.Count];
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = new Vector3(test[i].x, test[i].y, 0);
        
        return vertices;
    }

    private int[] getTriangles(Voronoi voronoi)
    {
        List<int> triangles = new List<int>();
        List<Site> sites = voronoi.Sites()._sites;

        foreach (Triangle triang in voronoi.Triangles())
        {
            triangles.Add(sites.IndexOf(triang.sites[0]));
            triangles.Add(sites.IndexOf(triang.sites[1]));
            triangles.Add(sites.IndexOf(triang.sites[2]));
        }
        return triangles.ToArray();
    }




    /*private void CopyInitialSprite(int vertInd0, int vertInd1, int vertInd2)
    {
        GameObject newGO = new GameObject("Test Mesh");

        var mf = newGO.AddComponent<MeshFilter>();
        var mr = newGO.AddComponent<MeshRenderer>();

        mr.material = GetMat(spriteRend);
        mf.mesh = SpriteToMesh(spriteRend.sprite);

        var coll = newGO.AddComponent<PolygonCollider2D>();
        coll.SetPath(0, new Vector2[] { mf.mesh.vertices[vertInd0], mf.mesh.vertices[vertInd1], mf.mesh.vertices[vertInd2] });

        //Graphics.DrawMeshNow(SpriteToMesh(spriteRend.sprite), Vector2.zero, Quaternion.identity, 0);
    }*/


    private Material GetMat(SpriteRenderer sRend)
    {
        Material mat = new Material(sRend.sharedMaterial);
        mat.SetTexture("_MainTex", sRend.sprite.texture);
        mat.color = sRend.color;
        return mat;
    }

    public void activateAndExplodePieces(Vector2 pos, Vector2 nextPos, int dmgForce, float radius, float maxDestroyPercent)
    {
        List<Collider2D> remainingPieces = new List<Collider2D>();
        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
            if (c.enabled) remainingPieces.Add(c);
        

        //instead of bullet pos take the piece as center that is nearest (since bullet size can change)... ideally would have contact point but projectile is a trigger
        Vector2 nearestPiecePos = Vector2.zero;
        float nearestPiecesDist = float.MaxValue;

        //c.ClosestPoint
        foreach (Collider2D c in remainingPieces)
        {
            float dist = Vector2.Distance(c.transform.position, pos);
            //print(c.transform.position);
            if (dist < nearestPiecesDist)
            {
                nearestPiecesDist = dist;
                nearestPiecePos = c.transform.position;
            }
        }

        // ensure nearest piece is most furstest on the edge on this side, so don't start explosion in the middle
        Vector2 finalPiecePos = nearestPiecePos;
        float furtestPiecesDist = 0;
        foreach (Collider2D c in remainingPieces)
        {           
            float dist = Vector2.Distance(c.transform.position, transform.position);

            if (dist > furtestPiecesDist)
            {
                // check if this piece is in angle somewhat to original piece
                if (Vector2.Angle(c.transform.position - transform.position, nearestPiecePos - (Vector2)transform.position) < 22.5f)
                {
                    furtestPiecesDist = dist;
                    finalPiecePos = c.transform.position;
                }
            }  
        }


        Collider2D[] piecesToDetach = Physics2D.OverlapCircleAll(finalPiecePos, radius);
        print(piecesToDetach.Length);
        /*
        while (piecesToDetach.Length <= 5 && piecesToDetach.Length < transform.childCount - 1)
        {
            radius += 0.1f;
            piecesToDetach = Physics2D.OverlapCircleAll(nearestPiecePos, radius);
        }*/

        // safety check so it can never be destroyed, but still having health

        if (piecesToDetach.Length + 5 >= remainingPieces.Count)
        {
            //print("should happen");
            return;
        }


        List<Rigidbody2D> rbs = new List<Rigidbody2D>();

        foreach (Collider2D c in piecesToDetach)
        {
            if (c.transform.IsChildOf(transform))
            {
                Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
                c.gameObject.tag = "Untagged";
                c.gameObject.layer = LayerMask.NameToLayer("Ignore Bullets");
                rb.isKinematic = false;
                rbs.Add(rb);
            }
        }

        //TODO: if too little peices come off it feels bad, so make the radius bigger if only some pieces came out
        // or get rid of pieces not connected to anything
        // OR, easiest way: only start radius search (above) from an outer edge piece, not in the middle?


        foreach (Rigidbody2D rb in rbs)
        {
            // TODO: add force in direction of bullet, but the more bullet points directly into object the more just send back to bullet
            //rb.AddForce((nextPos-pos).normalized * damage, ForceMode2D.Impulse);
            rb.AddForce((rb.transform.position - transform.position).normalized * dmgForce, ForceMode2D.Impulse);

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(rb.GetComponent<MeshRenderer>().material.DOFade(0, 0.5f));
            seq.AppendCallback(() => Destroy(rb.gameObject));
        }
    }


    public void destroy()
    {
        List<Rigidbody2D> rbs = new List<Rigidbody2D>();

        foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
        {
            rb.transform.parent = null;
            rb.gameObject.tag = "Untagged";
            rb.gameObject.layer = LayerMask.NameToLayer("Ignore Bullets");
            rb.isKinematic = false;
            rbs.Add(rb);
        }

        foreach (Rigidbody2D rb in rbs)
        {
            rb.AddForce((rb.transform.position - transform.position).normalized * 40, ForceMode2D.Impulse);

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(rb.GetComponent<MeshRenderer>().material.DOFade(0, 0.5f));
            seq.AppendCallback(() => Destroy(rb.gameObject));
        }
    }
}
