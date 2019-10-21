using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuCursor : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private Vector2 moveInput;

    private GraphicRaycaster gr;
    private PointerEventData pointerEventData = new PointerEventData(null);


    void Start()
    {
        gr = FindObjectOfType<GraphicRaycaster>();
    }


    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        transform.position += new Vector3(moveInput.x, moveInput.y, 0).normalized * speed;

        // clamp to screen
        //Vector3 world = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //transform.position =  new Vector3(Mathf.Clamp(transform.position.x, -world.x, world.x),
        //                                 Mathf.Clamp(transform.position.y, -world.y, world.y), transform.position.z);
    }

    private void OnSelect()
    {
        if (gr == null) return;

        pointerEventData.position = transform.position;//Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            foreach(RaycastResult result in results)
            {
                if (result.gameObject.name == "Start Button") //TODO: bad practise via obj name?
                    FindObjectOfType<MenuManager>().Play();

                if (result.gameObject.name == "Char Up")
                    FindObjectOfType<MenuManager>().toggleCharacter(gameObject, 1);

                if (result.gameObject.name == "Char Down")
                    FindObjectOfType<MenuManager>().toggleCharacter(gameObject, -1);
            }
        }

    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}