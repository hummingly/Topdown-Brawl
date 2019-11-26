using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    private TeamManager teams;
    private Rigidbody2D rb;
    private TrailCopyConstraint trail;

    //[ColorUsage(true, true)] public Color shouldbeHDR = Color.white;

    public bool melee;
    [SerializeField] private bool moveWithPlayer;
    [SerializeField] private float moveDistY = 1;
    [SerializeField] private float meleeAnimDur = 0.25f;
    [SerializeField] private float fadeAt = 0.075f;
    [SerializeField] private float disableAt = 0.15f;
    [SerializeField] private float dmgOnCollAfterKnockFor = 0.2f;
    [SerializeField] private int extraDmgOnWallHit = 20;
    [SerializeField] private float extraDmgVelThresh = 1;
    [SerializeField] private float extraDmgMaxAngle = 45;
    [Space]
    private int damage = 10;
    [SerializeField] private Vector2 minMaxDist = new Vector2(3, 12);
    [SerializeField] private Vector2 minMaxDmg = new Vector2(10, 50);
    [SerializeField] private Vector2 minMaxScale = new Vector2(1, 3);
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
    private Vector2 startScale;
    private int bounceAmmount;
    private float bounceGain;
    private float extendTravelDist;
    private Vector3 lastVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailCopyConstraint>();
        startPos = transform.position;
        startScale = transform.localScale;
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
            seq.InsertCallback(fadeSpd/4, () => GetComponentInChildren<ParticleSystem>().Stop());
            seq.AppendCallback(() => Destroy(gameObject));


            //Very similar to melee fade above...
        }
    }

    private void LateUpdate()
    {
        // for snipe dmg depending on distance
        if (damage == 0)
        {
            float dist = Vector2.Distance(startPos, transform.position);
            float multi = ExtensionMethods.Remap(dist, minMaxDist.x, minMaxDist.y, minMaxScale.x, minMaxScale.y);
            transform.localScale = startScale * multi;
            trail.setMulti(multi);
        }
    }

    private void FixedUpdate()
    {
        lastVel = rb.velocity;
    }

    public void SetInfo(int damage, GameObject owner, float range, int am, float gain, Vector2 scale)
    {
        this.owner = owner;
        this.damage = damage;
        this.destroyRange = range;
        this.bounceAmmount = am;
        this.bounceGain = gain;
        transform.localScale = scale;
        startScale = scale;
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
            other.transform.parent.GetComponent<IDamageable>().ReduceHealth(damage, owner, transform.position, (Vector2)transform.position + rb.velocity * Time.deltaTime);
            this.enabled = false;
        }


        var hitPoint = other.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);

        if (melee)
        {
            FindObjectOfType<EffectManager>().meleeBulletDeathPartic(hitPoint, transform);
            //GetComponentInChildren<Collider2D>().enabled = false;
            //Destroy(gameObject);

            GetComponentInChildren<Collider2D>().enabled = false;
            GetComponentInChildren<SpriteMask>().transform.DOKill();
            GetComponentInChildren<SpriteRenderer>().DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Append(GetComponentInChildren<SpriteRenderer>().DOFade(0, 0.1f));
            seq.AppendCallback(() => Destroy(gameObject));
        }
        else
        {
            FindObjectOfType<EffectManager>().bulletDeathPartic(hitPoint, transform);

            Destroy(gameObject);
        }


        var damageAble = other.GetComponent<IDamageable>();

        if (damageAble)
        {
            var sameTeam = teams.FindPlayerTeam(owner) == teams.FindPlayerTeam(damageAble.gameObject);

            // only damage enemys (-1 case isn't caught)
            if (!sameTeam)
            {
                if (damageAble.GetComponent<BotTest>())
                    damageAble.GetComponent<BotTest>().GotHit(owner); //order important so bot loses agro on death

                damageAble.ReduceHealth(damage, owner);

                if(melee)
                {
                    damageAble.GetComponent<PlayerMovement>().tookMeleeDmg(owner, dmgOnCollAfterKnockFor, extraDmgOnWallHit, extraDmgVelThresh, extraDmgMaxAngle);
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
                if(teams.FindPlayerTeam(owner) != teams.FindPlayerTeam(damageAble.gameObject))
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


    // TODO: combine with onTrigger bcz much the same
    private void OnCollisionEnter2D(Collision2D other)
    {

        // hit a piece of a destructible block, only one peice trigger per bullet tho so not too much dmg
        if (other.gameObject.tag == "Destruction Piece" && this.enabled)
        {
            other.transform.parent.GetComponent<IDamageable>().ReduceHealth(damage, owner, transform.position, (Vector2)transform.position + rb.velocity * Time.deltaTime);
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
        {
            var hitPoint = other.contacts[0].point;
            FindObjectOfType<EffectManager>().bulletDeathPartic(hitPoint, transform);

            Destroy(gameObject);
        }


        var damageAble = other.gameObject.GetComponent<IDamageable>();

        if (damageAble)
        {
            var sameTeam = teams.FindPlayerTeam(owner) == teams.FindPlayerTeam(damageAble.gameObject);

            // only damage enemys (-1 case isn't caught)
            if (!sameTeam)
            {
                if (damageAble.GetComponent<BotTest>())
                    damageAble.GetComponent<BotTest>().GotHit(owner); //order important so bot loses agro on death

                damageAble.ReduceHealth(damage, owner);
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
                if (teams.FindPlayerTeam(owner) != teams.FindPlayerTeam(damageAble.gameObject))
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
