using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffectTest : MonoBehaviour
{
    [SerializeField] private Material effectMat;

    void Awake()
    {
        effectMat.SetFloat("_AspectRatio", Camera.main.aspect);
        print(effectMat.GetFloat("_AspectRatio"));

    }

    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effectMat);
    }
}
