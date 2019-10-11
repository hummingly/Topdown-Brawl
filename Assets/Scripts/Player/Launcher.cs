using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed = 4;
    [SerializeField] private float spawnPosFromCenter = 0.5f;



    void Start()
    {
    
    }

    void Update()
    {

    }

    public void shoot(Vector2 shootDir) // Don't shoot where player really faces, but where he tries to look at with stick
    {
        GameObject p = Instantiate(projectile, (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter), Quaternion.identity);
        p.transform.up = shootDir;
        p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }
}
