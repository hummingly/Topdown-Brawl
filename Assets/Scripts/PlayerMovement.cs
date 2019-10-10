using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 rotInput;
    private Vector2 velocity;
    private Vector2 acc;

    [SerializeField] private PIDController torquePID;
    private Rigidbody2D rb;
    private Launcher launcher;
    private PlayerStats stats;

    [SerializeField] private float accForce;

    [SerializeField] private float maxVelocity;
    [SerializeField] private float accSpeed;     // speed of acc going to maxAcc
    [SerializeField] private float maxAcc;
    [SerializeField] private float nAccSpeed;    // speed of acc going back to 0
    [SerializeField] private float drag;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        launcher = GetComponent<Launcher>();
    }

    private void FixedUpdate()
    {
        //TODO: addforce? smooth and restrict diagonal
        velocity = rb.velocity;
        //rb.velocity = getVelocity(); // = new Vector2(moveInput.x, moveInput.y) * 10; 
        rb.AddForce(moveInput * accForce, ForceMode2D.Impulse);

        // TODO: instead add torque for physics! OR smooth visually
        if (rotInput != Vector2.zero)
            rotateToRightStick(); //  TODO: only if joystick fully pressed change look dir?

        // TODO: take rotInput directly to shoot bullets, don't wait for physical rotation
    }


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
    private void OnRightTrigger()
    {
        launcher.shoot();
    }



    private void rotateToRightStick()
    {
        // Works, but not physically accurate
        //transform.up = rotInput; 

        // Works the same
        //float desiredRot = Mathf.Atan2(rotInput.y, rotInput.x) * Mathf.Rad2Deg - 90f; 
        //transform.rotation = Quaternion.AngleAxis(desiredRot, Vector3.forward);
        
        // Better physicallity by changing rigidbody torque
        float desiredRot = Mathf.Atan2(rotInput.y, rotInput.x) * Mathf.Rad2Deg;// - 90f;
        float actualRot = transform.eulerAngles.z; // 0 to 360, but should be 180 to -180 like desiredRot
        actualRot -= 180;

        if (actualRot < -90 && actualRot > -180) actualRot += 270;
        else actualRot -= 90;

        // Look how far needs to rotate
        float dist = Mathf.Abs(actualRot - desiredRot); //distance between desired and actual rotation


        //if (dist >= 360)
        //    print("err 2"); // TODO: fix weird start error


        // Always rotate around the fastest side
        if (dist > 180)
        {
            dist -= 180;

            // Account for jump from 180 to -180
            if ((desiredRot > 90 && actualRot < -90) || (desiredRot < -90 && actualRot > 90)) 
                dist -= 180; 
        }


        float correction = torquePID.Update(0, dist, Time.deltaTime);
        rb.AddTorque(correction * Mathf.Sign(actualRot - desiredRot));

        //Debug.DrawRay(transform.position, rotInput * 10f, Color.blue); //Desired look dir
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(desiredRot, Vector3.forward) * transform.up * 5f, Color.red); //Desired look dir (the same as above, but using float instead of vector)
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(correction, Vector3.forward) * transform.up * 5f, Color.yellow); //Correction
    }


    private Vector2 getVelocity()
    {
        // Nachteil: auch bei kleinem input (oder wenig tweak des sticks des controllers) beschleunigt man auf maxVelocity (zwar langsamer, aber trotzdem)
        //      --> Lösung: beschleunigung nur bei "full-throttle" anwenden

        // x und y beschleunigung ist getrennt, deswegen ist es etwas schwieriger zu steuern

        acc.x = Math.Abs(moveInput.x) + acc.x;//doAcceleration(moveInput.x, acc.x);
        acc.y = Math.Abs(moveInput.y) + acc.y;//doAcceleration(moveInput.y, acc.y);

        // clamp max speed
        acc.x = Mathf.Clamp(acc.x, -maxAcc, maxAcc);
        acc.y = Mathf.Clamp(acc.y, -maxAcc, maxAcc);


        // apply drag
        velocity.x *= drag;
        velocity.y *= drag;

        Vector2 newVelocity = velocity + (moveInput * acc);

        // clamp diagonal movement
        Vector2 normVelocity = newVelocity.normalized;
        float absVelocity = newVelocity.magnitude;
        newVelocity = (absVelocity > maxVelocity) ? (normVelocity * maxVelocity) : newVelocity;

        return newVelocity;
    }

    /*private float doAcceleration(float input, float acc)
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
        if (dmgObj)
        {
            stats.ReduceHealth(dmgObj.damage);
            Vector2 pushDir = (Vector2)transform.position - collision.ClosestPoint(transform.position); // maybe instead solid collider? so that I can get hit point... but then player  can't really go "into" spikes
            rb.AddForce(pushDir * dmgObj.knockback, ForceMode2D.Impulse);
        }
    }
}
