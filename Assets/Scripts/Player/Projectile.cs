using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private TeamManager teams;

    private int damage;
    private GameObject owner;

    void Start()
    {
        teams = FindObjectOfType<TeamManager>();
    }

    void Update()
    {

    }

    public void setOwner(GameObject owner)
    {
        this.owner = owner;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag != "Bullet") // If its not another bulelt or the cinemachine confiner (now done via layer)

        Destroy(gameObject);

        var damageAble = collision.GetComponent<IDamageable>();

        if (damageAble)
        {
            // only damage enemys or neutrals
            var sameTeam = teams.getTeamOf(owner) == teams.getTeamOf(damageAble.gameObject);

            if(!sameTeam)
            {
                if (damageAble.GetComponent<BotTest>())
                    damageAble.GetComponent<BotTest>().gotHit(owner); //order important so bot loses agro on death

                bool didKill = damageAble.ReduceHealth(damage);

                if (didKill)
                {
                    FindObjectOfType<GameLogic>().increaseScore(owner);
                }
            }
        }
    }
}
