using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private TeamManager teams;
    private GameStateManager gameState;

    public List<Character> availableChars = new List<Character>();

    [SerializeField] private GameObject inputPrompt;
    [SerializeField] private GameObject botPrompt;
    [SerializeField] private Transform charSlotParent;
    [SerializeField] private Transform cursorParent;
    [SerializeField] private GameObject playerSlotPrefab;
    [SerializeField] private Image mapImg;

    void Awake()
    {
        teams = FindObjectOfType<TeamManager>();
        gameState = FindObjectOfType<GameStateManager>();
    }

    public void SetMapImg(Sprite sp)
    {
        mapImg.sprite = sp;
    }

    void Update()
    {
        // CHECK IF ALL PLAYERS READY
    }

    public void ToggleReady(GameObject player)
    {
        PlayerSlotMenuDisplay[] slots = charSlotParent.GetComponentsInChildren<PlayerSlotMenuDisplay>();
        foreach (var slot in slots)
        {
            if (slot.myPlayer == player)
            {
                var ready = slot.transform.GetComponentInChildren<Toggle>();
                ready.isOn = !ready.isOn;
                return;
            }
        }
    }

    public void ToggleCharacter(GameObject player, GameObject toggleButton, int dir)
    {
        var slotGO = toggleButton.transform.parent.parent;
        var slot = slotGO.GetComponent<PlayerSlotMenuDisplay>();

        // only change on own button (everyone can change bot)
        if (slot.myPlayer == player || slot.isBot)//(getSlotInd(slotGO) == teams.getPlayerId(player) || slot.isBot)
        {
            var lastIndex = availableChars.IndexOf(slot.chara);
            lastIndex += dir;
            if (lastIndex < 0) lastIndex = availableChars.Count - 1;
            if (lastIndex >= availableChars.Count) lastIndex = 0;
            slot.SetChar(availableChars[lastIndex]);
        }


        //TODO: actually spawn different character based on selection
    }

    public void TogglePlayerTeam(GameObject player/*that toggled*/, GameObject toggleButton)
    {
        var slotGO = toggleButton.transform.parent;
        var slot = slotGO.GetComponent<PlayerSlotMenuDisplay>();

        // only change on own button 
        if (slot.myPlayer == player) //(getSlotInd(slotGO) == teams.getPlayerId(player))
        {
            teams.MoveTeam(player);
            slot.SetCol(teams.GetColorOf(player));
            slot.myPlayer.GetComponent<MenuCursor>().SetColor(teams.GetColorOf(player));
        }

        // everyone can change bot
        if (slot.isBot)
        {
            var bot = slot.myPlayer;//teams.getPlayerByID(getSlotInd(slotGO)); //possibly instable, prone to bugs ?!?!?!

            // Instead of going through each team go through all colors from when bot placed, and then delete
            slot.botDeleteCounter++;

            // Already max team, so remove instead (after cycle through all colors)
            if (slot.botDeleteCounter >= teams.teams.Count)//(teams.getTeamOf(bot) + 1 >= teams.teams.Count)
            {
                teams.Remove(bot);//remove bot cursor in team list

                //replace the slot with an empty fill one
                var index = slot.transform.GetSiblingIndex();
                Destroy(slot.gameObject);
                Transform empty = new GameObject("Fill", typeof(RectTransform)).transform;
                empty.parent = charSlotParent;
                empty.SetSiblingIndex(index);

                Destroy(bot);//remove bot cursor
            }
            else
            {
                teams.MoveTeam(bot);
                slot.SetCol(teams.GetColorOf(bot));
            }
        }
    }

    public void ToggleMap()
    {
        gameState.ToggleMap();
    }

    public void ToggleGameMode(GameObject button)
    {
        string name = gameState.ToggleGameMode();
        TextMeshProUGUI textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.SetText(name);
    }

    public void PlayerJoined(Transform playerCursor, bool isBot = false, int place = 0)
    {
        if (inputPrompt.activeInHierarchy)
        {
            inputPrompt.SetActive(false);
            botPrompt.SetActive(true);
        }

        playerCursor.parent = cursorParent;
        playerCursor.localPosition = Vector3.zero;

        if (!isBot)
        {
            playerCursor.GetComponent<MenuCursor>().Setup(teams.playerNrs.IndexOf(playerCursor.gameObject), teams.GetColorOf(playerCursor.gameObject));
        }

        var replaced = false;

        //add as many empty slots as needed so bot spawns where pressed
        if (isBot)
        {
            //replace that empty GO with bot
            if (place < charSlotParent.childCount)
            {
                Destroy(charSlotParent.GetChild(place).gameObject);

                replaced = true;
            }
            else // place new empty GO(s)
            {
                int offSet = place - charSlotParent.childCount;
                for (int i = 0; i < offSet; i++)
                {
                    Transform empty = new GameObject("Fill", typeof(RectTransform)).transform;
                    empty.parent = charSlotParent;
                }
            }
        }

        var slot = Instantiate(playerSlotPrefab, transform.position, Quaternion.identity).transform;
        slot.parent = charSlotParent;
        slot.GetComponent<PlayerSlotMenuDisplay>().SetSlot(playerCursor, availableChars[0], teams.GetColorOf(playerCursor.gameObject), isBot, teams.playerNrs.IndexOf(playerCursor.gameObject));

        // for bot
        if (replaced)
            slot.transform.SetSiblingIndex(place);


        //for player on joining if full (but with bots or empty GOs)
        if (!isBot)
        {
            var currentPlayerCount = teams.GetTotalPlayers();
            if (currentPlayerCount > 1)
            {
                currentPlayerCount--; //since just joined one
            }

            if (currentPlayerCount < 6) //TODO: add max player size dynamically... and enforce it too
            {
                // if current team count is still smaller than 6, but the children on the slotParent are already 6, get rid of the first empty GO...
                if (charSlotParent.childCount >= 6)
                {
                    for (int i = 0; i < charSlotParent.childCount; i++)
                    {
                        if (!charSlotParent.GetChild(i).GetComponent<PlayerSlotMenuDisplay>())
                        {
                            Destroy(charSlotParent.GetChild(i).gameObject);
                            slot.transform.SetSiblingIndex(i);
                            break;
                        }
                    }
                }
            }
            else //if team count is 6, get rid of first bot
            {
                for (int i = 0; i < charSlotParent.childCount; i++)
                    if (charSlotParent.GetChild(i).GetComponent<PlayerSlotMenuDisplay>().isBot)
                    {
                        teams.Remove(charSlotParent.GetChild(i).GetComponent<PlayerSlotMenuDisplay>().myPlayer);//remove bot cursor in team list

                        //TODO: missing step? everything of bot deleted?????

                        Destroy(charSlotParent.GetChild(i).gameObject);
                        slot.transform.SetSiblingIndex(i);
                        break;
                    }
                //TODO: delete player again if no bot to delete...
            }
        }
        slot.transform.localScale = Vector3.one;
    }

    public void Play()
    {
        gameState.Play();
    }

    // should be the index of the player or bot here, ignoring all empty objects in the list that are for correct palcement
    private int GetSlotInd(Transform slotGO)
    {
        // count all the empty fill slots that help add a bot all the way on the right on the grid for example
        int emptySlots = 0;

        for (int i = 0; i < charSlotParent.childCount; i++)
            if (charSlotParent.GetChild(i).GetComponent<PlayerSlotMenuDisplay>() == null)
                emptySlots++;

        return slotGO.GetSiblingIndex() - emptySlots;
    }

    public Character GetCharacterOfPlayer(GameObject player)
    {
        for (int i = 0; i < charSlotParent.childCount; i++)
        {
            var slot = charSlotParent.GetChild(i).GetComponent<PlayerSlotMenuDisplay>();

            if (slot != null && slot.myPlayer == player)
            {
                return slot.chara;
            }
        }
        return null;
    }
}
