using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCopyConstraint : MonoBehaviour //TODO: ColorCopyConstraint in general
{ 
    //[SerializeField] Renderer rend;
    //[SerializeField] Renderer fixTo;

    [SerializeField] TrailRenderer rend;
    [SerializeField] SpriteRenderer fixTo;

    private float[] alphaOffsets;

    private float startWidth;
    private float multi = 1;

    private void Awake()
    {
        startWidth = rend.widthMultiplier;

        alphaOffsets = new float[rend.colorGradient.alphaKeys.Length];

        for (int i = 0; i < rend.colorGradient.alphaKeys.Length; i++)
            alphaOffsets[i] = fixTo.color.a - rend.colorGradient.alphaKeys[i].alpha;
    }

    private void LateUpdate()
    {
        //rend.material.color = fixTo.material.color;
        //rend.material.SetColor("_TintColor", fixTo.material.color);


        //if(rend == TrailRenderer)


        /*Gradient gradient = rend.colorGradient;//new Gradient();
        for(int i = 0; i < gradient.colorKeys.Length; i++)
            gradient.colorKeys[i].color.a = fixTo.color.a;

        rend.colorGradient = gradient;*/


        /*GradientAlphaKey[] alphaKeys = new GradientAlphaKey[rend.colorGradient.alphaKeys.Length];
        for (int i = 0; i < rend.colorGradient.alphaKeys.Length; i++)
        {
            //print(rend.colorGradient.alphaKeys[i].alpha);
            //rend.colorGradient.alphaKeys[i] = new GradientAlphaKey(0, rend.colorGradient.alphaKeys[i].time);
            alphaKeys[i] = new GradientAlphaKey(0, rend.colorGradient.alphaKeys[i].time);
        }
        rend.colorGradient.SetKeys(rend.colorGradient.colorKeys, alphaKeys);*/




        /* working unity example
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        rend.colorGradient = gradient;*/



        // expect 3 alpha keys, make first two scale with sprite and last one kept at org offset... nvm do dynamic
        //float alpha = fixTo.color.a;

        //GradientColorKey[] colorKeys = new GradientColorKey[rend.colorGradient.colorKeys.Length];
        //for (int i = 0; i < rend.colorGradient.colorKeys.Length; i++)
        //    colorKeys[i] = new GradientColorKey(rend.colorGradient.colorKeys[i].color, rend.colorGradient.colorKeys[i].time);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[rend.colorGradient.alphaKeys.Length];
        for (int i = 0; i < rend.colorGradient.alphaKeys.Length; i++)
        {
            float alpha = fixTo.color.a - alphaOffsets[i];
            alphaKeys[i] = new GradientAlphaKey(alpha, rend.colorGradient.alphaKeys[i].time);
        }

        Gradient gradient = new Gradient();
        gradient.SetKeys(rend.colorGradient.colorKeys, alphaKeys);

        rend.colorGradient = gradient;



        //rend.widthCurve = startWidth * 1;
        rend.widthMultiplier = startWidth * multi;
    }

    public void setMulti(float am)
    {
        multi = am;
    }
}
