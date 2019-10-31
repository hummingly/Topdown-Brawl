using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelationImageEffect : ImageEffectTest
{
    //[SerializeField] private float pixelCount = 64;

    void Awake()
    {
        //effectMat.SetFloat("_PixelAmmount", pixelCount);
        effectMat.SetFloat("_AspectRatio", Camera.main.aspect);
        //print(effectMat.GetFloat("_AspectRatio"));
    }

    void Update()
    {
        
    }
}
