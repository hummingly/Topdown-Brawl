using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private float dashUsedBrigthness = 0.5f;

    private Color mainColor;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void InitColor(Color col)
    {
        SetColorAll(col);
        mainColor = col;
    }

    public void SetActionOnCooldownCol()
    {
        var darkColor = mainColor;
        float H;
        float S;
        float V;
        Color.RGBToHSV(darkColor, out H, out S, out V);
        V = dashUsedBrigthness;
        darkColor = Color.HSVToRGB(H,S,V);
        //print(darkColor);
        SetColorAll(darkColor);
    }
    public void SetMainColor()
    {
        SetColorAll(mainColor);
    }

    private void SetColorAll(Color col)
    {
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.color = col;
    }
}
