using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private TeamManager teams;

    [SerializeField] private int damage = 10;
    [Space]
    [SerializeField] private float knockStrength = 20;
    [SerializeField] private float keepOrgVel = 0.8f;
    [Space]
    [SerializeField] private float knockStrengthPlayer = 50;
    [SerializeField] private float keepOrgVelPlayer = 0.1f;

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        //if(collision.tag != "Bullet") // If its not another bulelt or the cinemachine confiner (now done via layer)

        Destroy(gameObject);

        var damageAble = other.GetComponent<IDamageable>();

        if (damageAble)
        {
            var sameTeam = teams.getTeamOf(owner) == teams.getTeamOf(damageAble.gameObject);

            // only damage enemys (-1 case isn't caught)
            if (!sameTeam)
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

        // only knock back enemys or neutrals
        var otherRb = other.GetComponent<Rigidbody2D>();
        if (otherRb)//(rigidbod && teams.getTeamOf(other.gameObject) == -1)
        {
            var knockDir = other.ClosestPoint(transform.position/*maybe rather bullet tip?*/) - (Vector2)transform.position;
            knockDir = knockDir.normalized;

            // Player
            if (other.GetComponent<PlayerMovement>())
            {
                if(teams.getTeamOf(owner) != teams.getTeamOf(damageAble.gameObject))
                {
                    //otherRb.velocity = Vector2.zero;
                    otherRb.AddForce(knockDir * knockStrengthPlayer, ForceMode2D.Impulse); //TODO: instead of value for objects & for enemys: factor in mas and drag
                }
            }
            else
            {
                otherRb.velocity = Vector2.zero;
                otherRb.AddForce(knockDir * knockStrength, ForceMode2D.Impulse); //TODO: instead of value for objects & for enemys: factor in mas and drag
            }
        }
    }
}
