using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    void Start()
    {
        setColorAll(TeamManager.instance.getRandUnusedColor());
    }

    void Update()
    {
        
    }

    private void setColorAll(Color col)
    {
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.color = col;
    }
}
