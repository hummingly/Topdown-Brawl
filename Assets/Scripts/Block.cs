using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour // need mono?
{
    private Rigidbody2D rb;
    private bool doesMove;
    //[SerializeField] LevelManager.BlockType blockType; // NEEDED?


    public void init(Vector2 scale, bool moveable, float weight, bool destructible, Color color)
    {
        //gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<SpriteRenderer>().color = color;
        transform.localScale = new Vector3(scale.x, scale.y, 1);

        if (moveable)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.mass = weight;
        }
    }


    public void startMoving()
    {
        doesMove = true;
        // TODO: start moving in a direction
    }

    void Update()
    {
        
    }
}
