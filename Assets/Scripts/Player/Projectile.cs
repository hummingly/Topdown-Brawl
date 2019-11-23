using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    private TeamManager teams;
    private Rigidbody2D rb;

    public bool melee;
    [SerializeField] private bool moveWithPlayer;
    [SerializeField] private float moveDistY = 1;
    [SerializeField] private float meleeAnimDur = 0.25f;
    [SerializeField] private float fadeAt = 0.075f;
    [SerializeField] private float disableAt = 0.15f;
    [Space]
    private int damage = 10;
    [SerializeField] private Vector2 minMaxDist = new Vector2(3, 12);
    [SerializeField] private Vector2 minMaxDmg = new Vector2(10, 50);
    [Space]
    [SerializeField] private float knockStrength = 20;
    [SerializeField] private float keepOrgVel = 0.8f;
    [Space]
    [SerializeField] private float knockStrengthPlayer = 50;
    [SerializeField] private float keepOrgVelPlayer = 0.1f;
    [Space]
    [SerializeField] private float fadeSpd = 0.5f;
    [SerializeField] private float fadeSlowdown = 0.8f;

    private GameObject owner;
    private float destroyRange;
    private Vector2 startPos;
    private int bounceAmmount;
    private float bounceGain;
    private float extendTravelDist;
    private Vector3 lastVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Start()
    {
        teams = FindObjectOfType<TeamManager>();
        if (melee)
        {
            if (moveWithPlayer)
                transform.parent = owner.transform;

            //transform.DOLocalMoveY(moveDistY, meleeAnimDur);
            transform.DOMove(transform.position + transform.up * moveDistY, meleeAnimDur);

            Sequence seq = DOTween.Sequence();
            seq.Append(GetComponentInChildren<SpriteMask>().transform.DOLocalMoveY(0, meleeAnimDur));
            seq.Insert(fadeAt, GetComponentInChildren<SpriteRenderer>().DOFade(0, meleeAnimDur - fadeAt));
            //seq.AppendCallback(() => Destroy(gameObject));
            seq.InsertCallback(disableAt, () => GetComponentInChildren<Collider2D>().enabled = false); 
            seq.AppendCallback(() => Destroy(gameObject));

            //TODO: add easing
        }
    }

    void Update()
    {
        if (Vector2.Distance(startPos, transform.position) - extendTravelDist >= destroyRange)
        {
            rb.velocity *= fadeSlowdown; //slow down speed, more ideally OVER TIME

            foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
                s.DOFade(0, fadeSpd).SetEase(Ease.OutQuad);

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(fadeSpd);
            seq.InsertCallback(fadeSpd/4, () => GetComponentInChildren<Collider2D>().enabled = false);
            seq.AppendCallback(() => Destroy(gameObject));


            //Very similar to melee fade above...
        }
    }

    private void FixedUpdate()
    {
        lastVel = rb.velocity;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetRange(float range)
    {
        this.destroyRange = range;
    }

    public void SetBounce(int am, float gain)
    {
        this.bounceAmmount = am;
        this.bounceGain = gain;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //if(collision.tag != "Bullet") // If its not another bulelt or the cinemachine confiner (now done via layer)


        // for snipe dmg depending on distance
        if (damage == 0)
        {
            float dist = Vector2.Distance(startPos, transform.position);
            damage = Mathf.RoundToInt(ExtensionMethods.Remap(dist, minMaxDist.x, minMaxDist.y, minMaxDmg.x, minMaxDmg.y));
            print(dist + " " + damage);
        }


        // hit a piece of a destructible block, only one peice trigger per bullet tho so not too much dmg
        if (other.tag == "Destruction Piece" && this.enabled)
        {
            other.transform.parent.GetComponent<IDamageable>().ReduceHealth(damage, transform.position, (Vector2)transform.position + rb.velocity * Time.deltaTime);
            this.enabled = false;
        }


        if (melee)
            GetComponentInChildren<Collider2D>().enabled = false;
        else
            Destroy(gameObject);

        var damageAble = other.GetComponent<IDamageable>();

        if (damageAble)
        {
            var sameTeam = teams.GetTeamOf(owner) == teams.GetTeamOf(damageAble.gameObject);

            // only damage enemys (-1 case isn't caught)
            if (!sameTeam)
            {
                if (damageAble.GetComponent<BotTest>())
                    damageAble.GetComponent<BotTest>().GotHit(owner); //order important so bot loses agro on death

                bool didKill = damageAble.ReduceHealth(damage);

                if (didKill)
                {
                    FindObjectOfType<GameLogic>().IncreaseScore(owner);
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
                if(teams.GetTeamOf(owner) != teams.GetTeamOf(damageAble.gameObject))
                {
                    //otherRb.velocity = Vector2.zero;
                    otherRb.velocity *= keepOrgVelPlayer; // doesnt work well since player always changed velocity...
                    otherRb.AddForce(knockDir * knockStrengthPlayer, ForceMode2D.Impulse); //TODO: instead of value for objects & for enemys: factor in mas and drag
                }
            }
            else // Object
            {
                //otherRb.velocity = Vector2.zero;
                otherRb.velocity *= keepOrgVel; // doesnt work well since player always changed velocity...
                otherRb.AddForce(knockDir * knockStrength, ForceMode2D.Impulse); //TODO: instead of value for objects & for enemys: factor in mas and drag
                //otherRb.AddForceAtPosition(knockDir * knockStrength, other.ClosestPoint(transform.position), ForceMode2D.Impulse); 
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        // hit a piece of a destructible block, only one peice trigger per bullet tho so not too much dmg
        if (other.gameObject.tag == "Destruction Piece" && this.enabled)
        {
            other.transform.parent.GetComponent<IDamageable>().ReduceHealth(damage, transform.position, (Vector2)transform.position + rb.velocity * Time.deltaTime);
            this.enabled = false;
        }


        if (bounceAmmount >= 0)
        {
            bounceAmmount--;
            extendTravelDist += bounceGain;
            print(other.gameObject.name);
            //other.ClosestPoint.n
            rb.velocity = Vector2.Reflect(lastVel, other.contacts[0].normal);
        }
        else
            Destroy(gameObject);


        var damageAble = other.gameObject.GetComponent<IDamageable>();

        if (damageAble)
        {
            var sameTeam = teams.GetTeamOf(owner) == teams.GetTeamOf(damageAble.gameObject);

            // only damage enemys (-1 case isn't caught)
            if (!sameTeam)
            {
                if (damageAble.GetComponent<BotTest>())
                    damageAble.GetComponent<BotTest>().GotHit(owner); //order important so bot loses agro on death

                bool didKill = damageAble.ReduceHealth(damage);

                if (didKill)
                {
                    FindObjectOfType<GameLogic>().IncreaseScore(owner);
                }
            }
        }



        // only knock back enemys or neutrals
        var otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb)//(rigidbod && teams.getTeamOf(other.gameObject) == -1)
        {
            var knockDir = other.contacts[0].point - (Vector2)transform.position;
            knockDir = knockDir.normalized;

            // Player
            if (other.gameObject.GetComponent<PlayerMovement>())
            {
                if (teams.GetTeamOf(owner) != teams.GetTeamOf(damageAble.gameObject))
                {
                    //otherRb.velocity = Vector2.zero;
                    otherRb.velocity *= keepOrgVelPlayer; // doesnt work well since player always changed velocity...
                    otherRb.AddForce(knockDir * knockStrengthPlayer, ForceMode2D.Impulse); //TODO: instead of value for objects & for enemys: factor in mas and drag
                }
            }
            else // Object
            {
                //otherRb.velocity = Vector2.zero;
                otherRb.velocity *= keepOrgVel; // doesnt work well since player always changed velocity...
                otherRb.AddForce(knockDir * knockStrength, ForceMode2D.Impulse); //TODO: instead of value for objects & for enemys: factor in mas and drag
                //otherRb.AddForceAtPosition(knockDir * knockStrength, other.ClosestPoint(transform.position), ForceMode2D.Impulse); 
            }
        }



    }
}
