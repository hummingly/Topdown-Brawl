using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        var damageAble = collision.collider.GetComponent<IDamageable>();
        if(damageAble)
        {
            damageAble.ReduceHealth(damage);
        }
    }
}
