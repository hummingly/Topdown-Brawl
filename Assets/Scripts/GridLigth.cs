using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLigth
{

    public Transform centerTransform;
    public SpriteRenderer spriteRend;
    //float intensity;
    //float range;


    public GridLigth(SpriteRenderer s, Transform t)
    {
        spriteRend = s;
        centerTransform = t;
    }

}
