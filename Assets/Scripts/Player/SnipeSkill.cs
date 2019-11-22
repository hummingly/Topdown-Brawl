using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeSkill : Skill
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed;
    private AimLaser aimLaser;
    //private float lastInput;
    private float aimVal;

    void Start ()
    {
        aimLaser = gameObject.GetComponentInChildren<AimLaser>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void Attack(Vector2 shootDir)
    {
        GameObject p = Instantiate(projectile, (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter), Quaternion.identity);
        p.GetComponent<Projectile>().SetDamage(damage);
        p.GetComponent<Projectile>().SetOwner(gameObject);
        p.transform.up = shootDir;
        p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }


    private void LateUpdate()
    {
        if (aimVal > 0 && delayTimer <= 0)
            aimLaser.SetAim(true);
        else
            aimLaser.SetAim(false);
    }

    protected override void OnTrigger(float inputValue)
    {
        // bug for permanent aiming very difficult to reproduce now (almost impossible)
        /*if (inputValue < inputTolerance && delayTimer <= 0)
        {
            //print(inputValue);
            aimLaser.setAim(true);
        }
        else
        {
            aimLaser.setAim(false);
        }*/

        //shootInput = inputValue;
        //lastInput = inputValue;

        aimVal = inputValue;
    }



    protected override void OnTriggerUp(float inputValue)
    {

        //shootInput = inputValue; //1;
        if (delayTimer <= 0)
            DoAttack();
    }

    protected override void OnTriggerDown()
    {

    }
}
