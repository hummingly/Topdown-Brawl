using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDestruction : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private PolygonCollider2D sourcePolyCollider;

    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        sourcePolyCollider = GetComponent<PolygonCollider2D>();

        CopyInitialSprite(0,1,2);
        //copyInitialSpriteSplitBy(1);
    }

    /*
    private void CopyInitialSpriteSplitBy(int splitPoints)
    {
        /*var currVerts = sourcePolyCollider.GetPath(0).Length;
        var newTris = currVerts * splitPoints - (currVerts - 1); // 1 new point: 3 tris, 2 new points: 5 tris, 3 new points: 7 tris


        List<Vector2> points = new List<Vector2>(sourcePolyCollider.GetPath(0));

        // add middle point since we start with a triangle
        points.Add((points[0] + points[1] + points[2]) / 3);

        copyInitialSprite(0,1,3);
        copyInitialSprite(0,2,3);
        copyInitialSprite(1,2,3);*/

        /*for (int i = 0; i < newTris; i++)
        {
            //put a random point somewhere between existing points
            //calculate the new 3 triangles points and create the 3 objects
        }*/



        //for (int i = 0; i < extraPoints; i++)
        //    points.Add(new Vector2(Random.Range(rect.width / -2, rect.width / 2), Random.Range(rect.height / -2 + rect.center.y, rect.height / 2 + rect.center.y)));


        /*Voronoi voronoi = new Delaunay.Voronoi(points, null, rect);

        List<List<Vector2>> clippedTriangles = new List<List<Vector2>>();
        foreach (Triangle tri in voronoi.Triangles())
        {
            clippedTriangles = ClipperHelper.clip(borderPoints, tri);
            foreach (List<Vector2> triangle in clippedTriangles)
            {
                pieces.Add(generateTriangularPiece(source, triangle, origVelocity, origScale, origRotation, mat));
            }
        }

        foreach (piece in pieces)
            copyInitialSprite(...);
        
    }*/


    //https://www.google.com/search?safe=active&rlz=1C1CHBF_deDE832DE832&sxsrf=ACYBGNSFRh7fqJnp7-h4lL-LAtxj8805BA%3A1571516804708&ei=hHGrXbbwKsnIwALEv7OAAw&q=unity+shatter+mesh&oq=unity+shatter+mesh&gs_l=psy-ab.3..0i203.4067.6745..6826...3.4..1.77.748.11......0....1..gws-wiz.......0i71j0i13j0i7i30j0i7i10i30j0i7i30i19j0i19j0i13i30i19j0i8i13i30i19.p30JUBtEeF8&ved=0ahUKEwi2wpynlKnlAhVJJFAKHcTfDDAQ4dUDCAs&uact=5


    private void CopyInitialSprite(int vertInd0, int vertInd1, int vertInd2)
    {
        GameObject newGO = new GameObject("Test Mesh");

        var mf = newGO.AddComponent<MeshFilter>();
        var mr = newGO.AddComponent<MeshRenderer>();

        mr.material = GetMat(spriteRend);
        mf.mesh = SpriteToMesh(spriteRend.sprite);

        var coll = newGO.AddComponent<PolygonCollider2D>();
        coll.SetPath(0, new Vector2[] { mf.mesh.vertices[vertInd0], mf.mesh.vertices[vertInd1], mf.mesh.vertices[vertInd2] });

        //Graphics.DrawMeshNow(SpriteToMesh(spriteRend.sprite), Vector2.zero, Quaternion.identity, 0);
    }


    private Mesh SpriteToMesh(Sprite sprite)
    {
        Mesh mesh = new Mesh();

        List<Vector2> points = new List<Vector2>();

        var test = sourcePolyCollider.GetPath(0);
        for (int i = 0; i < test.Length; i++)
            points.Add(test[i]);


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
        mesh.triangles = triangles;/**/


        //mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
        //mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);

        //mesh.uv = sprite.uv; //mf.mesh.uv = calcUV(vertices, spriteRend, transform); // TODO: if there is a texture also use uv


        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        //mesh.RecalculateTangents();

        //print(points.Count);
        //print(mf.mesh.vertices.Length); // **** problem is that its 7 and not 3 vertices anymore, as it was regenerated...

        return mesh;
    }

    private Material GetMat(SpriteRenderer sRend)
    {
        Material mat = new Material(sRend.sharedMaterial);
        mat.SetTexture("_MainTex", sRend.sprite.texture);
        mat.color = sRend.color;
        return mat;
    }
}
