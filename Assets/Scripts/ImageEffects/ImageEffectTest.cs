using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class ImageEffectTest : MonoBehaviour //maybe abstract, then children inherit but also set the values eg. pixel count to shader
{
    [SerializeField] protected Material effectMat;

    void Awake()
    {

    }

    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effectMat);
    }
}
