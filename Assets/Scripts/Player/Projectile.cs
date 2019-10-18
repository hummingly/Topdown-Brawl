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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag != "Bullet") // If its not another bulelt or the cinemachine confiner
        Destroy(gameObject);

        var damageAble = collision.GetComponent<IDamageable>();
        if (damageAble)
        {
            damageAble.ReduceHealth(damage);
        }
    }
}
