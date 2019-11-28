using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private int bounceAmMax;
    [SerializeField] private float bounceGain;
    [SerializeField] private int splitAm;
    [SerializeField] private Vector2 scale = new Vector2(1,1);
    //[SerializeField] private Vector2 scaleMax = new Vector2(1, 1);

    private void Start()
    {
        if (showIndicationOnHold)
        {
            aimLaser = gameObject.GetComponentInChildren<AimLaser>();
            aimLaser.setDist(range);
        }
    }

    protected override void Action(Vector2 shootDir)
    {
        var spawnPos = (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter);
        shootDir = ExtensionMethods.RotatePointAroundPivot(shootDir, Vector2.zero, Random.Range(-accuracy, accuracy));
        GameObject p = Instantiate(projectile, spawnPos, Quaternion.identity);
        p.GetComponent<Projectile>().SetInfo(damage, gameObject, range, bounceAmMax, bounceGain, scale);
        p.transform.up = shootDir;
        //p.transform.eulerAngles += new Vector3(0,0,90);//Random.Range(-accuracy, accuracy)
        if (!p.GetComponent<Projectile>().melee)
            p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());

        if (!p.GetComponent<Projectile>().melee)
        {
            float dmg = damage;
            if (dmg == 0) dmg = 75;
            effects.muzzle(dmg, p.transform, gameObject);
            effects.AddShake(0.3f, shootDir, 0.25f);

            if (p.GetComponent<Projectile>().sniperShot)
            {
                effects.snipeShot(spawnPos, p.transform, gameObject, GetComponent<PlayerInput>() == null ? null : (Gamepad)GetComponent<PlayerInput>().devices[0]);
                effects.AddShake(0.5f, shootDir);
            }
        }
        else
            effects.meleeBlow(transform);
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

    public void disableLaser()
    {
        if (showIndicationOnHold)
            aimLaser.SetAim(false);
        //aimVal = 0;
    }
}
