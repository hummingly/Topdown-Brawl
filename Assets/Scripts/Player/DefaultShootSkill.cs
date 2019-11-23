using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShootSkill : Skill
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private AimLaser aimLaser; //TODO: general range indication class
    [SerializeField] private float speed;

    private float aimVal;

    [SerializeField] private int bulletsPerShot = 1;
    [SerializeField] private int maxDelayBetweenBullets;
    [SerializeField] private float bulletLikelyDelay; //0.0 to 1.0
    [SerializeField] private float accuracy;
    [SerializeField] private float range = 4;
    [SerializeField] private bool doesPierce;
    [SerializeField] private int bounceAm;
    [SerializeField] private int splitAm;
    [SerializeField] private float scale = 1;

    private void Start()
    {
        if(showIndicationOnHold)
            aimLaser = gameObject.GetComponentInChildren<AimLaser>();
    }

    protected override void Action(Vector2 shootDir)
    {
        GameObject p = Instantiate(projectile, (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter), Quaternion.identity);
        p.GetComponent<Projectile>().SetDamage(damage);
        p.GetComponent<Projectile>().SetOwner(gameObject);
        p.transform.up = shootDir;
        if(!p.GetComponent<Projectile>().melee)
            p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }

  

    protected override void OnTrigger(float inputValue) // may be integrated into the OnRight- / OnZRightTrigger methods
    {
        if(showIndicationOnHold)
            aimVal = inputValue;
        else
            actionInput = inputValue;
    }



    private void LateUpdate()
    {
        if(showIndicationOnHold)
        {
            if (aimVal > 0 && delayTimer <= 0)
                aimLaser.SetAim(true);
            else
                aimLaser.SetAim(false);
        }
    }
}
