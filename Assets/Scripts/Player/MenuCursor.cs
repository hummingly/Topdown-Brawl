using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuCursor : MonoBehaviour
{
    private readonly float speed = 0.01f;
    [SerializeField] private Image spriteTeamCol;
    [SerializeField] private TextMeshProUGUI playerNrText;

    private Vector2 moveInput;

    private GraphicRaycaster gr;
    private PointerEventData pointerEventData = new PointerEventData(null);
    private MatchMaker matchMaker;

    private void Start()
    {
        gr = FindObjectOfType<GraphicRaycaster>();
        matchMaker = FindObjectOfType<MatchMaker>();
    }

    public void Setup(int playerNr, Color teamColor)
    {
        playerNrText.text = "P" + (playerNr + 1).ToString("0");
        SetColor(teamColor);
    }

    public void SetColor(Color teamColor)
    {
        spriteTeamCol.color = teamColor;
    }

    private void FixedUpdate()
    {
        var dir = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += Vector3.ClampMagnitude(dir, 1) * speed * Screen.width;
    }

    private void OnSelect()
    {
        if (gr == null)
        {
            gr = FindObjectOfType<GraphicRaycaster>();
        }

        pointerEventData.position = transform.position;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            var emptySlot = true;
            GameObject addBotButton = null;

            foreach (RaycastResult hitObj in results)
            {
                //TODO: bad practise via obj name?
                switch (hitObj.gameObject.name)
                {
                    case "Char Up":
                        matchMaker.ToggleCharacter(gameObject, hitObj.gameObject, 1);
                        break;
                    case "Char Down":
                        matchMaker.ToggleCharacter(gameObject, hitObj.gameObject, -1);
                        break;
                    case "Change Team Button":
                        matchMaker.TogglePlayerTeam(gameObject, hitObj.gameObject);
                        emptySlot = false;
                        break;
                    case "Map Button Toggle":
                        matchMaker.ToggleMap();
                        break;
                    case "Add Bot Button":
                        addBotButton = hitObj.gameObject;
                        break;
                    case "Game Mode Toggle":
                        matchMaker.ToggleGameMode(hitObj.gameObject);
                        break;
                }
            }

            if (addBotButton && emptySlot)
            {
                FindObjectOfType<MatchData>().TeamManager.AddBot();
            }
        }

    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnReady(InputValue value)
    {
        Debug.Log("Ready");
        var state = FindObjectOfType<GameStateManager>();
        if (state.State == GameStateManager.GameState.Start)
        {
            state.MakeMatch();
        }
        else if (state.State == GameStateManager.GameState.MatchMaking)
        {
            FindObjectOfType<MatchMaker>().ToggleReady(gameObject);
        }
    }

    private void OnLeave(InputValue value)
    {
        Debug.Log("Leaving Team.");
    }
}