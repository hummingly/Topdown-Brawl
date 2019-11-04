using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Awake()
    {
        teams = FindObjectOfType<TeamManager>();
        gameState = FindObjectOfType<GameStateManager>();
    }


    void Update()
    {
        // CHECK IF ALL PLAYERS READY
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
        if(slot.isBot)
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

                print("del bot");
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




    public void PlayerJoined(Transform playerCursor, bool isBot = false, int place = 0)
    {
        if (inputPrompt.active)
        {
            inputPrompt.SetActive(false);
            botPrompt.SetActive(true);
        }

        playerCursor.parent = cursorParent;
        playerCursor.localPosition = Vector3.zero;

        if (!isBot) playerCursor.GetComponent<MenuCursor>().Setup(teams.playerNrs.IndexOf(playerCursor.gameObject)/*teams.getPlayerId(playerCursor.gameObject)*/, teams.GetColorOf(playerCursor.gameObject));
  

        var replaced = false;

        //add as many empty slots as needed so bot spawns where pressed
        if(isBot)
        {
            //replace that empty GO with bot
            if (place < charSlotParent.childCount)
            {
                Destroy(charSlotParent.GetChild(place).gameObject);

                replaced = true;
            }
            else
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

        if(replaced)
            slot.transform.SetSiblingIndex(place);
    }

    public void Play()
    {
        teams.SaveCharacters();
        FindObjectOfType<UnityEngine.InputSystem.PlayerInputManager>().joinBehavior = UnityEngine.InputSystem.PlayerJoinBehavior.JoinPlayersManually;
        gameState.state = GameStateManager.GameState.Ingame;
        LoadMap();
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(FindObjectOfType<GameStateManager>().currentMapInd);
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
