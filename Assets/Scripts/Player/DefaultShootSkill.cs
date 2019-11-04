using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShootSkill : Skill
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed;

    protected override void Attack(Vector2 shootDir)
    {
        GameObject p = Instantiate(projectile, (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter), Quaternion.identity);
        p.GetComponent<Projectile>().SetDamage(damage);
        p.GetComponent<Projectile>().SetOwner(gameObject);
        p.transform.up = shootDir;
        p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }

    protected override void OnTrigger(float inputValue)
    {
        // may be integrated into the OnRight- / OnZRightTrigger methods
        actionInput = inputValue;
    }

    protected override void OnTriggerUp(float inputValue)
    {

    }
}
