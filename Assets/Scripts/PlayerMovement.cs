using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 rotInput;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private Vector2 acc;

    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    // speed of acc going to maxAcc
    private float accSpeed;

    [SerializeField]
    private float maxAcc;

    [SerializeField]
    // speed of acc going back to 0
    private float nAccSpeed;

    [SerializeField]
    private float drag;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        maxVelocity = 8;
        acc = new Vector2(0, 0);
        accSpeed = 0.1f;
        nAccSpeed = 0.3f;
        maxAcc = 1;
        drag = 0.5f;
    }




    private void FixedUpdate()
    {
        velocity = rb.velocity;
        rb.velocity = /*new Vector2(moveInput.x, moveInput.y)*/getVelocity()/*moveInput * 10*/; //TODO: addforce? smooth and restrict diagonal

        if (rotInput != Vector2.zero)
            transform.up = rotInput; // TODO: instead add torque for physics! and smooth visually
    }



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
    
    // make value approach 0 in steps of the size of interval
    private float approachZero(float value, float interval)
    {
        if (value == 0)
        {
            return value;
        }
        if (value < 0)
        {
            float newValue = value + interval;
            if (newValue > 0)
            {
                newValue = 0;
            }
            return newValue;
        }
        else
        {
            float newValue = value - interval;
            if (newValue < 0)
            {
                newValue = 0;
            }
            return newValue;
        }
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
    }

    private Vector2 getVelocity()
    {

        // Nachteil: auch bei kleinem input (oder wenig tweak des sticks des controllers) beschleunigt man auf maxVelocity (zwar langsamer, aber trotzdem)
        //      --> Lösung: beschleunigung nur bei "full-throttle" anwenden

        // x und y beschleunigung ist getrennt, deswegen ist es etwas schwieriger zu steuern

        acc.x = doAcceleration(moveInput.x, acc.x);
        acc.y = doAcceleration(moveInput.y, acc.y);

        // clamping
        acc.x = acc.x > maxAcc ? maxAcc : acc.x < -maxAcc ? -maxAcc : acc.x;
        acc.y = acc.y > maxAcc ? maxAcc : acc.y < -maxAcc ? -maxAcc : acc.y;
        
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

        //clamping
        Vector2 normVelocity = newVelocity.normalized;
        float absVelocity = newVelocity.magnitude;
        newVelocity = absVelocity > maxVelocity ? normVelocity * maxVelocity : newVelocity;

        return newVelocity;

        /*

        Vector2 newVelocity = moveInput * acc;
        if (newVelocity.magnitude > maxVelocity)
        {
            return newVelocity.normalized * maxVelocity;
        }
        return newVelocity;*/
    }

    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html#started-performed-and-canceled-callbacks
    //https://www.youtube.com/watch?v=D8nUI88POU8&t=4s

    //TODO: implement deviceLost -> pause game ?
}
