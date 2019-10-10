using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed = 4;



    void Start()
    {
    
    }

    void Update()
    {

    }

    public void shoot(Vector2 shootDir) // Don't shoot where player really faces, but where he tries to look at with stick
    {
        GameObject p = Instantiate(projectile, transform.position, transform.rotation);
        p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }
}
