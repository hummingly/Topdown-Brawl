using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour // need mono?
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Tweener t;
    private DestructibleBlock destruct;

    private Vector3 orgScale;

    // TODO: factor the properties like destructible out to other scripts
    //public bool isDestructible;
    //public int maxHealth;
    [Space]
    public bool doesMove;
    [Space]
    public bool doesBounce;
    public float bounceAm = 200;
    //[SerializeField] private LevelManager.BlockType blockType; // NEEDED?


    private void Awake()
    {
        if (doesBounce)
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            orgScale = sprite.transform.localScale;
        }
        if (GetComponent<Rigidbody2D>())
            rb = GetComponent<Rigidbody2D>();
    }



    public void setColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color; // or make public and set directly?
    }

    public void setMoveable(float weight)
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.mass = weight;
    }
    public void setDestructible(float hp)
    {
        // TODO
    }

    public void startMoving()
    {
        doesMove = true;
        // TODO: start moving in a direction
    }

    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = collision.collider.GetComponent<Rigidbody2D>();

        if (doesBounce && rb)
        {
            rb.AddForce((rb.transform.position - transform.position).normalized * bounceAm, ForceMode2D.Impulse); //TODO: add option to preserve last velocity before contact

            if (t != null && t.IsPlaying())
            {
                t.Kill();
                sprite.transform.localScale = orgScale;
            }

            t = sprite.transform.DOPunchScale(Vector3.one, 0.25f); //TODO: add shakeScale && make sure no deformation stays
        }

        /*
        if(isDestructible)
        {
            var projectile = collision.collider.GetComponent<Projectile>();
            if(projectile)
            {

            }
            // TODO: add possibilty to get damaged if its the player himself flying against this
        }
        */
    }
}
