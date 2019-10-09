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

    [SerializeField] private float accForce;

    [SerializeField] private float maxVelocity;
    [SerializeField] private float accSpeed;     // speed of acc going to maxAcc
    [SerializeField] private float maxAcc;
    [SerializeField] private float nAccSpeed;    // speed of acc going back to 0
    [SerializeField] private float drag;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //TODO: addforce? smooth and restrict diagonal
        velocity = rb.velocity;
        //rb.velocity = getVelocity(); // = new Vector2(moveInput.x, moveInput.y) * 10; 
        rb.AddForce(moveInput * accForce, ForceMode2D.Impulse);

        // TODO: instead add torque for physics! and smooth visually
        if (rotInput != Vector2.zero)
            transform.up = rotInput; 
            //rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, 0, 1) * Time.deltaTime));
            //rb.AddTorque(Vector2.Angle(transform.forward, rotInput)); 
            //rb.AddTorque(Vector2.Angle(transform.forward, rotInput)); 
            //transform.localRotation.eulerAngles = Vector3.Slerp(transform.forward, rotInput, 1 * Time.deltaTime);
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

}
