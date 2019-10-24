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
            var emptySlot = true;
            var addBot = false;

            foreach(RaycastResult hitObj in results)
            {
                if (hitObj.gameObject.name == "Start Button") //TODO: bad practise via obj name?
                    FindObjectOfType<MenuManager>().Play();

                if (hitObj.gameObject.name == "Char Up")
                    FindObjectOfType<MenuManager>().toggleCharacter(gameObject, hitObj.gameObject, 1);

                if (hitObj.gameObject.name == "Char Down")
                    FindObjectOfType<MenuManager>().toggleCharacter(gameObject, hitObj.gameObject, - 1);


                if (hitObj.gameObject.name == "Change Team Button")
                {
                    FindObjectOfType<MenuManager>().togglePlayerTeam(gameObject, hitObj.gameObject);
                    emptySlot = false;
                }

                if (hitObj.gameObject.name == "Add Bot Button")
                    addBot = true;
            }

            if (addBot && emptySlot)
                FindObjectOfType<TeamManager>().addBot();//FindObjectOfType<MenuManager>().addBot();
        }

    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}