using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSkill : Skill
{
    private Rigidbody2D rb;

    protected enum Type { Dash, Teleport, SpeedChange };
    protected Type type = Type.Dash;


    [SerializeField] private float dashForce = 300;
    [SerializeField] protected bool doesDamage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    protected override void Action(Vector2 shootDir)
    {
        // if not using left stick to move, dash to look direction or right stick
        Vector2 dashDir;
        if (playerMovement.GetMoveInput() != Vector2.zero)
            dashDir = playerMovement.GetMoveInput();
        else
            dashDir = playerMovement.GetLastRot();

        rb.AddForce(dashDir * dashForce, ForceMode2D.Impulse);


        effects.DoDash(transform.position, dashDir, transform);
    }



    protected override void OnTrigger(float inputValue)
    {

    }

}
