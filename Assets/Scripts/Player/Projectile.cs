using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private TeamManager teams;

    [SerializeField] private int damage = 10;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag != "Bullet") // If its not another bulelt or the cinemachine confiner (now done via layer)

        Destroy(gameObject);

        var damageAble = collision.GetComponent<IDamageable>();

        if (damageAble)
        {
            // only damage enemys or neutrals
            var sameTeam = teams.getTeamOf(owner) == teams.getTeamOf(damageAble.gameObject);
            //print(teams.getTeamOf(damageAble.gameObject));
            //print(teams.getTeamOf(owner));
            if(!sameTeam)
            {
                bool playerKilled = damageAble.ReduceHealth(damage);

                if (playerKilled)
                {
                    teams.increaseScore(owner);
                }
            }
        }
    }
}
