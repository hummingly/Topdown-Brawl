using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{

    public int damage;
    public float knockback;
    [Space]
    public bool damageOnContact;
    [Space]
    public bool damageOnSqueeze;
    public float squeezeSafetyRoom = 0.1f;
    public int ammountOfRays = 2;
    [Space]
    public bool damageOnPush;
    public float pushForce;
    public float pushForceSide;

    private MovingObject movement;
    private Collider2D myColl;

    private void Awake()
    {
        myColl = GetComponent<Collider2D>();
        if (GetComponent<MovingObject>())
            movement = GetComponent<MovingObject>();
    }

    private void Update()
    {
        if(damageOnSqueeze)
        {
            // Raycast in moving dir
            var moveDir = movement.getMoveDir();

            for(int j = -ammountOfRays + 1; j < ammountOfRays; j++)
            {
                var pointA = ((Vector2)transform.position + moveDir * (transform.localScale.y / 2)) + new Vector2(moveDir.y, moveDir.x) * j * (transform.localScale.x / (ammountOfRays - 1) / 2);

                RaycastHit2D[] rayHit = Physics2D.RaycastAll(pointA, moveDir, squeezeSafetyRoom);

                Debug.DrawLine(pointA, pointA + moveDir * squeezeSafetyRoom, Color.white);

                bool closeEnough = false;

                // if one of the rays is very close to a block, check if player is between, then kill
                for (int i = 0; i < rayHit.Length; i++)
                {
                    // neither player nor this block
                    if (rayHit[i].collider != myColl && !rayHit[i].collider.GetComponent<IDamageable>())
                    {
                        closeEnough = true;
                        break;
                    }
                }

                if (closeEnough)
                {
                    for (int i = 0; i < rayHit.Length; i++)
                    {
                        // TODO: add certain regulations like no squashing on triggers?
                        if (rayHit[i].collider.GetComponent<IDamageable>())
                            rayHit[i].collider.GetComponent<IDamageable>().ReduceHealth(int.MaxValue);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageOnPush)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable)
            {
                var moveDir = movement.getMoveDir();

                for (int j = -ammountOfRays + 1; j < ammountOfRays; j++)
                {
                    RaycastHit2D[] rayHit = Physics2D.RaycastAll(((Vector2)transform.position + moveDir * (transform.localScale.y / 2)) + new Vector2(moveDir.y, moveDir.x) * j * (transform.localScale.x / (ammountOfRays - 1) / 2), moveDir, squeezeSafetyRoom);

                    for (int i = 0; i < rayHit.Length; i++)
                    {
                        if (rayHit[i].collider != myColl && rayHit[i].collider.GetComponent<IDamageable>() == damageable)
                        {
                            damageable.ReduceHealth(damage);
                            var rb = damageable.GetComponent<Rigidbody2D>();
                            if (rb)
                                rb.AddForce(moveDir * pushForce + new Vector2(moveDir.y * ExtensionMethods.randNegPos(), moveDir.x * ExtensionMethods.randNegPos()) * pushForceSide, ForceMode2D.Impulse);
                        }
                    }
                }

                // TODO: add multiplier for damage depending on speed? also knockback depend on speed?
            }
        }
    }
}
