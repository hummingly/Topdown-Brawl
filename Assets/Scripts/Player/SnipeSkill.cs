using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeSkill : Skill
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed;
    [SerializeField] private float aimTolerance;
    private AimLaser aimLaser;

    void Start ()
    {
        // which ones does it get?? multiple players?????????
        //aimLaser = GameObject.FindObjectOfType<AimLaser>();
        // doesnt work
        aimLaser = gameObject.GetComponentInChildren<AimLaser>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void Attack(Vector2 shootDir)
    {
        GameObject p = Instantiate(projectile, (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter), Quaternion.identity);
        p.GetComponent<Projectile>().setDamage(damage);
        p.GetComponent<Projectile>().setOwner(gameObject);
        p.transform.up = shootDir;
        p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }

    protected override void OnTrigger(float inputValue)
    {
        // may be integrated into the OnRight- / OnZRightTrigger methods
        if (inputValue > aimTolerance && inputValue < inputTolerance)
        {
            print("AIMIN");
            if (aimLaser == null) 
                print("NULLLLLLLL AIMLASER");
            aimLaser.setAim(true);
        } else
        {
            aimLaser.setAim(false);
        }
        shootInput = inputValue;
    }
}
