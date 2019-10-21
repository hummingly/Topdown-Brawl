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

    public void initColor(Color col)
    {
        setColorAll(col);
        mainColor = col;
    }

    public void setDashUsedColor()
    {
        var darkColor = mainColor;
        float H;
        float S;
        float V;
        Color.RGBToHSV(darkColor, out H, out S, out V);
        V = dashUsedBrigthness;
        darkColor = Color.HSVToRGB(H,S,V);
        //print(darkColor);
        setColorAll(darkColor);
    }
    public void setMainColor()
    {
        setColorAll(mainColor);
    }

    private void setColorAll(Color col)
    {
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.color = col;
    }
}
