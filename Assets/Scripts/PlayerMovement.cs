using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 rotInput;
    private Rigidbody2D rb;
        
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }




    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x, moveInput.y) * 10; //TODO: addforce? smooth and restrict diagonal

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

    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html#started-performed-and-canceled-callbacks
    //https://www.youtube.com/watch?v=D8nUI88POU8&t=4s

    //TODO: implement deviceLost -> pause game ?
}
