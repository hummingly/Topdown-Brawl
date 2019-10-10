using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed = 4;

    // Use this for initialization
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void shoot()
    {
        GameObject p = Instantiate(projectile, transform.position, transform.rotation);
        p.GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }
}
