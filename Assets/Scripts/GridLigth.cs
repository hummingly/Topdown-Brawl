using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLigth
{

    public Transform centerTransform;
    public SpriteRenderer spriteRend;
    public float maxIntensity;
    public float intensity;
    public float range;


    public GridLigth(float i, float r, SpriteRenderer s, Transform t)
    {
        maxIntensity = i;
        intensity = i;
        range = r;
        spriteRend = s;
        centerTransform = t;
    }

    public void updateIntensity()
    {
        intensity = maxIntensity * spriteRend.color.a;
    }
}
