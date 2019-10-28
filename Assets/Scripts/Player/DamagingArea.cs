using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagingArea : MonoBehaviour
{
    private TeamManager teams;

    [SerializeField] private int damage = 20;
    [SerializeField] private GameObject owner;
    [SerializeField] private Image effectImg;
    private Collider2D coll;

    private float fillAm = 0.3f;
    [SerializeField] private float effectSpd = 0.2f;
    [SerializeField] private float disableAfter = 0.05f;
    [SerializeField] private float fadeSpd = 0.05f;
    [SerializeField] private float knockback = 50f;
    private float coolDown;

    void Start()
    {
        coll = GetComponent<Collider2D>();
        teams = FindObjectOfType<TeamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void activate()
    {
        //if already showing, hide for one frame then show again?


        effectImg.enabled = true;
        coll.enabled = true;

        effectImg.fillAmount = 0;
        effectImg.color = new Color(1,1,1,1);

        Sequence seq = DOTween.Sequence();
        seq.Append(effectImg.DOFillAmount(fillAm, effectSpd));
        seq.AppendInterval(disableAfter);
        seq.Append(effectImg.DOFade(0, fadeSpd));
        seq.AppendCallback(() => effectImg.enabled = false);
        seq.AppendCallback(() => coll.enabled = false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag != "Bullet") // If its not another bulelt or the cinemachine confiner (now done via layer)

        var damageAble = collision.GetComponent<IDamageable>();

        if (damageAble)
        {
            // only damage enemys or neutrals
            var sameTeam = teams.getTeamOf(owner) == teams.getTeamOf(damageAble.gameObject);

            if (!sameTeam)
            {
                if (damageAble.GetComponent<BotTest>())
                    damageAble.GetComponent<BotTest>().gotHit(owner); //order important so bot loses agro on death


                //knockback
                var dir = damageAble.transform.position - transform.position;
                damageAble.GetComponent<Rigidbody2D>().AddForce(dir.normalized * knockback, ForceMode2D.Impulse);


                bool didKill = damageAble.ReduceHealth(damage);

                if (didKill)
                {
                    FindObjectOfType<GameLogic>().increaseScore(owner);
                }
            }
        }
    }
}
