using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLightAddon : MonoBehaviour
{
    public float maxIntens = 0.1f;
    public float maxRange = 2.5f;
    private GridLigth light;

    void Start()
    {
        SpriteRenderer s = gameObject.AddComponent<SpriteRenderer>();
        //s.material.shader = Shader.Find("Sprites/Default");
        s.color = Color.white;
        light = FindObjectOfType<EffectManager>().AddGridLigth(maxIntens, maxRange, s, transform);
    }

    public void set(float i, float r)
    {
        light.maxIntensity = i;
        light.range = r;
    }
}
