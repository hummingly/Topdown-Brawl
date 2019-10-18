using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFake3D : MonoBehaviour
{
    private SpriteRenderer baseSpr;
    private Transform cam;

    [SerializeField] private Color wallColor = Color.grey;
    [SerializeField] private int steps = 2;
    [SerializeField] private float maxOffset = 0.5f;

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
                prepareNewSprite(ref tops, "Top", 2, orgCol);
            else
                prepareNewSprite(ref tops, "Top", 1, wallColor);
        }
        for (int i = 0; i < steps; i++)
        {
            prepareNewSprite(ref bots, "Bot", -1, wallColor);
        }


    }


    // Rotate top and bottom sprites so that it looks 3d from the camera pos
    void LateUpdate()
    {
        for (int i = 1; i <= tops.Count; i++)
        {
            tops[i - 1].transform.localPosition = (transform.position - cam.position).normalized;// new Vector2(0,1);
            tops[i - 1].transform.localPosition = new Vector2(tops[i - 1].transform.localPosition.x / transform.localScale.x,
                                                              tops[i - 1].transform.localPosition.y / transform.localScale.y);
            tops[i - 1].transform.localPosition = tops[i - 1].transform.localPosition * maxOffset * ((float)i / tops.Count);
    }

        for (int i = 1; i <= tops.Count; i++)
        {
            bots[i - 1].transform.localPosition = (cam.position - transform.position).normalized;// new Vector2(0,1);
            bots[i - 1].transform.localPosition = new Vector2(bots[i - 1].transform.localPosition.x / transform.localScale.x,
                                                              bots[i - 1].transform.localPosition.y / transform.localScale.y);
            bots[i - 1].transform.localPosition = bots[i - 1].transform.localPosition * maxOffset * ((float)i / tops.Count);
        }
    }


    private void prepareNewSprite(ref List<Transform> ts, string name, int orderInLayer, Color col)
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
