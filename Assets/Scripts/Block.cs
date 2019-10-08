using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour // need mono?
{
    private Rigidbody2D rb;

    void init(bool moveable, float weight, bool destructible, Sprite sprite)
    {
        if (moveable)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.mass = weight;
            gameObject.AddComponent<Collider2D>();
        }
    }



    void Update()
    {
        
    }
}
