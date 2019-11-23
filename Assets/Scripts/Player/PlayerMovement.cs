using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 rotInput;
    private float velocity;
    //private Vector2 acc;

    private Vector2 lastRotInput;


    //[SerializeField] private float maxRotSpd = 1000;

    [SerializeField] private Vector2 startRot;
    [SerializeField] private float inputStartRotThresh;
    [SerializeField] private PIDController torquePID;
    private Rigidbody2D rb;
    private PlayerStats stats;


    [SerializeField] private float accForce;

    //[SerializeField] private float maxVelocity;
    //[SerializeField] private float accSpeed;     // speed of acc going to maxAcc
    //[SerializeField] private float maxAcc;
    //[SerializeField] private float nAccSpeed;    // speed of acc going back to 0
    //[SerializeField] private float drag;

    // used fields just for debugging purposes
    private float breathing;
    private int counter;
    private int breathSpeed;
    private float correction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();

        // Set init rotation
        lastRotInput = startRot;
        transform.up = startRot;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        // TODO: move a lot to update
        velocity = rb.velocity.magnitude;
        float div1 = Vector2.Dot(lastRotInput, moveInput);
        float div2 = lastRotInput.magnitude * moveInput.magnitude;
        float cos = div1 / div2;
        float rad = Mathf.Acos(cos);
        //a = rad * Mathf.Rad2Deg;

        //float acc = (a > 120) ? (backVelocity) : accForce;
        float acc = accForce - rad;
        //rb.AddForce(moveInput * acc, ForceMode2D.Impulse);
        rb.AddForce(moveInput * accForce, ForceMode2D.Impulse);

        
        // TODO: instead add torque for physics! OR smooth visually
        RotateToRightStick(); //  TODO: only if joystick fully pressed change look dir?

        //breathSpeed = GetBreathSpeed();
        //counter = (counter + 1) % breathSpeed;

        // TODO: take rotInput directly to shoot bullets, don't wait for physical rotation
    }
    /*
    private int GetBreathSpeed()
    {
        if (velocity < 4)
        {
            return 25;
        }
        else
        {
            return 15;
        }
        //return Mathf.RoundToInt(velocity * 4f);
    }

    private float GetBreathStrength()
    {
        if (velocity < 1)
        {
            return 0;
        }
        else if (velocity < 4)
        {
            return 100;
        }
        else
        {
            return 200;
        }
        //return velocity * 3;
    }

    private float AddBreathing(float desiredRot)
    {
        if (counter == 0)
        {
            if (breathing > 0)
                breathing = GetBreathStrength() * -1;
            else
                breathing = GetBreathStrength()*5;
            return desiredRot + breathing;
        }
        return desiredRot;
    }
    */

    // TODO: refactor out into input script
    private void OnA()
    {
        print("hit a");
    }
    
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    private void OnRotate(InputValue value)
    {
        rotInput = value.Get<Vector2>();
    }


    // bot setters
    public void SetMove(Vector2 val)
    {
        moveInput = val;
    }
    public void SetRot(Vector2 val)
    {
        rotInput = val;
    }


    public Vector2 GetLastRot()
    {
        return lastRotInput;
    }
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }


    private void RotateToRightStick()
    {
        // Always rotate to last input dir
        if (rotInput.magnitude > inputStartRotThresh) // 0 is immediety input, but weird cause snaps to L/R/U/D
            lastRotInput = rotInput;

        float desiredRot = Mathf.Atan2(lastRotInput.y, lastRotInput.x) * Mathf.Rad2Deg + 90f; // 90 to convert the transform rot to the input rot
        //desiredRot = addBreathing(desiredRot);
        
        float actualRot = transform.eulerAngles.z; // 0 to 360, but should be 180 to -180 like desiredRot
        actualRot -= 180;

        if (desiredRot > 180)
            desiredRot -= 360;

        // Look how far needs to rotate for ideal pos
        float dist = Mathf.Abs(actualRot - desiredRot);

        // Always rotate around the fastest side
        if (dist > 180)
        {
            dist -= 180;

            // Account for jump from 180 to -180
            if ((desiredRot > 90 && actualRot < -90) || (desiredRot < -90 && actualRot > 90))
                dist -= 180;
        }

        // Always apply rotation so that player looks to last direction that was input
        correction = torquePID.Update(0, dist, Time.deltaTime);
        //correction = addBreathing(correction);
        //correction = Mathf.Clamp(correction, 0, maxRotSpd);
        rb.AddTorque(correction * Mathf.Sign(actualRot - desiredRot));

        //probably outdated:
        Debug.DrawRay(transform.position, rotInput * 10f, Color.blue); //Desired look dir
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(desiredRot, Vector3.forward) * Vector2.up * 5f, Color.red); //Desired look dir (the same as above, but using float instead of vector)
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(correction, Vector3.forward) * Vector2.up * 5f, Color.yellow); //Correction
        //print(correction);
    }

    /*
    private Vector2 getVelocity()
    {
        // Nachteil: auch bei kleinem input (oder wenig tweak des sticks des controllers) beschleunigt man auf maxVelocity (zwar langsamer, aber trotzdem)
        //      --> Lösung: beschleunigung nur bei "full-throttle" anwenden

        // x und y beschleunigung ist getrennt, deswegen ist es etwas schwieriger zu steuern

        acc.x = doAcceleration(moveInput.x, acc.x);
        acc.y = doAcceleration(moveInput.y, acc.y);

        // clamp max speed
        acc.x = Mathf.Clamp(acc.x, -maxAcc, maxAcc);
        acc.y = Mathf.Clamp(acc.y, -maxAcc, maxAcc);

        
        // apply drag
        if (velocity.x < 0)
        {
            velocity.x += drag;
            if (velocity.x > 0)
            {
                velocity.x = 0;
            }
        }
        else if (velocity.x > 0)
        {
            velocity.x -= drag;
            if (velocity.x < 0)
            {
                velocity.x = 0;
            }
        }
        if (velocity.y < 0)
        {
            velocity.y += drag;
            if (velocity.y > 0)
            {
                velocity.y = 0;
            }
        }
        else if (velocity.y > 0)
        {
            velocity.y -= drag;
            if (velocity.y < 0)
            {
                velocity.y = 0;
            }
        }

        Vector2 newVelocity = velocity + (moveInput * acc);

        // clamp max velocity (incl. diagonal movement)
        Vector2 normVelocity = newVelocity.normalized;
        float absVelocity = newVelocity.magnitude;

        float div1 = Vector2.Dot(lastRotInput, moveInput);
        float div2 = lastRotInput.magnitude * moveInput.magnitude;
        float cos = div1 / div2;
        float a = Mathf.Acos(cos);
        a *= Mathf.Rad2Deg;
        
        newVelocity = (a > 120) ? (normVelocity * backVelocity) : newVelocity;
        newVelocity = (absVelocity > maxVelocity) ? (normVelocity * maxVelocity) : newVelocity;
        return newVelocity;
    }

    private float doAcceleration(float input, float acc)
    {
        // wegen Ungenauigkeiten bei meinem Controller... kA, wie das bei euch ist??
        if (Math.Abs(input) > 0.1f)
        {
            return acc + accSpeed;
        }
        else
        {
            //return approachZero(acc, nAccSpeed);
            return 0;
        }
    }*/


    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html#started-performed-and-canceled-callbacks
    //https://www.youtube.com/watch?v=D8nUI88POU8&t=4s

    //TODO: implement deviceLost -> pause game ?
    /*private void OnDeviceLost()
    {
        print("lost");
        velocity = Vector2.zero;
    }
    private void OnDeviceRegained()
    {
        print("regained");
        velocity = Vector2.zero;
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var dmgObj = collision.GetComponent<DamagingObject>();
        if (dmgObj && dmgObj.damageOnContact)
        {
            stats.ReduceHealth(dmgObj.damage);
            Vector2 pushDir = (Vector2)transform.position - collision.ClosestPoint(transform.position); // maybe instead solid collider? so that I can get hit point... but then player  can't really go "into" spikes
            rb.AddForce(pushDir * dmgObj.knockback, ForceMode2D.Impulse);
        }
    }

    /*
     * // crusher worked ok, but didn't kill when fully closed
    private void OnCollisionStay2D(Collision2D collision)
    {
        DamagingObject dmgObj = collision.gameObject.GetComponent<DamagingObject>();
        if (dmgObj && dmgObj.damageOnSqueeze)
        {
            // Wait until other collider also overlaps with player (apart from the crushing box) !!! MAY cause problems with colliding bullets etc, so can add (solid) mask
            Collider2D[] hitColls = Physics2D.OverlapBoxAll(transform.position, transform.localScale / 2, transform.localRotation.z);

            for (int i = 0; i < hitColls.Length; i++)
            {
                //    Debug.Log("Hit : " + hitColls[i].name + i);
                //if(collision == solid)
                if (hitColls[i] != dmgObj.GetComponent<Collider2D>() && hitColls[i] != GetComponent<Collider2D>())
                {
                    stats.ReduceHealth(dmgObj.damage);
                    break;
                }
            }


        }
    }
    */
}
