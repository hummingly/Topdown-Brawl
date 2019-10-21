using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private GameObject owner;

    void Start()
    {

    }

    void Update()
    {

    }

    public void setOwner(GameObject owner)
    {
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag != "Bullet") // If its not another bulelt or the cinemachine confiner
        Destroy(gameObject);

        var damageAble = collision.GetComponent<IDamageable>();
        if (damageAble)
        {
            bool playerKilled = damageAble.ReduceHealth(damage);
            if (playerKilled)
            {
                GetComponent<GameLogic>().increaseScore(owner);
            }
        }
    }
}
