using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFake3D : MonoBehaviour
{
    private SpriteRenderer baseSpr;
    private Transform cam;

    [SerializeField] private Color wallColor = Color.grey;
    /*[SerializeField]*/ private Color shadowColor = new Color(0.2f,0.2f,0.2f);//0.15f //Color.black;
    [SerializeField] private int steps = 2;
    [SerializeField] private float maxOffset = 0.5f;
    ///*[SerializeField]*/ private float shadowExtraOffset = 0.005f;

    private Transform top;
    private Transform bot; // TODO: add multiple layers, not just 1 top 1 bot
    private List<Transform> tops = new List<Transform>();
    private List<Transform> bots = new List<Transform>();

    void Start()
    {
        cam = Camera.main.transform;
        baseSpr = GetComponent<SpriteRenderer>();

        var orgCol = baseSpr.color;
        baseSpr.color = wallColor;


        for (int i = 0; i < steps; i++)
        {
            // Top one different color
            if (i == steps-1)
                PrepareNewSprite(ref tops, "Top", 2, orgCol);
            else
                PrepareNewSprite(ref tops, "Top", 1, wallColor);
        }
        for (int i = 0; i < steps; i++)
        {
            PrepareNewSprite(ref bots, "Bot", -1, wallColor);
        }
        PrepareNewSprite(ref bots, "Bot", -2, shadowColor);
    }


    // Rotate the position of the top and bottom sprites so that it looks 3d from the camera pos
    void LateUpdate()
    {
        Vector2 topDir = (transform.position - cam.position).normalized;
        topDir = ExtensionMethods.RotatePointAroundPivot(topDir, Vector2.zero, -transform.eulerAngles.z);

        for (int i = 1; i <= tops.Count; i++)
        {
            var dir = new Vector2(topDir.x / transform.localScale.x,
                                  topDir.y / transform.localScale.y);
            dir *= maxOffset * ((float)i / tops.Count);

            tops[i - 1].transform.localPosition = dir;
        }
        
        for (int i = 1; i <= bots.Count; i++)
        {
            var dir = -topDir;
            dir = new Vector2(dir.x / transform.localScale.x,
                              dir.y / transform.localScale.y);

            dir *= maxOffset * ((float)i / bots.Count);

            // last one is shadow
            //if (i == bots.Count)
            //    dir += dir.normalized*shadowExtraOffset;

            bots[i - 1].transform.localPosition = dir;
        }
    }


    private void PrepareNewSprite(ref List<Transform> ts, string name, int orderInLayer, Color col)
    {
        var t = new GameObject(name).transform;
        ts.Add(t);
        t.transform.position = transform.position;
        t.transform.localScale = transform.localScale;
        t.transform.localRotation = transform.localRotation;
        t.parent = transform;
        var topSpr = t.gameObject.AddComponent<SpriteRenderer>();
        topSpr.sprite = baseSpr.sprite;
        topSpr.color = col;
        topSpr.sortingOrder = orderInLayer;
    }
}
