using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuCursor : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    void Start()
    {

    }


    void Update()
    {

    }


    private void OnSelect()
    {
        // check if below is button

        RaycastHit2D[] rayHit = Physics2D.RaycastAll(transform.position, Vector3.forward);


        for (int j = 0; j < rayHit.Length; j++)
        {
            if (rayHit[j].collider.GetComponent<Button>())
                print("YES");
        }
    }
    private void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        transform.position += (Vector3)moveInput.normalized * speed;
    }
}