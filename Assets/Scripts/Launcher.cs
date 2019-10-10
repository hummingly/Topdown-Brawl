using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
    public Rigidbody2D projectile;
    public float speed = 4;

    // Use this for initialization
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnA()
    {
        Debug.Log("SHOOT!");
        Rigidbody2D p = Instantiate(projectile, transform.position, transform.rotation);
        p.velocity = transform.forward * speed;
    }
}
