using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private TeamManager teams;
    private GameStateManager gameState;

    [SerializeField] private List<Character> availableChars = new List<Character>();
    
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



    public void toggleCharacter(GameObject player, GameObject toggleButton, int dir)
    {
        var slotGO = toggleButton.transform.parent.parent;
        var slot = slotGO.GetComponent<PlayerSlotMenuDisplay>();

        // only change on own button (everyone can change bot)
        if (slotGO.GetSiblingIndex() == teams.getPlayerId(player) || slot.isBot)
        {
            //var displaySlot = charSlotParent.GetChild(teams.getPlayerId(player)).GetComponent<PlayerSlotMenuDisplay>();
            var lastIndex = availableChars.IndexOf(slot.chara);
            lastIndex += dir;
            if (lastIndex < 0) lastIndex = availableChars.Count - 1;
            if (lastIndex >= availableChars.Count) lastIndex = 0;
            slot.setChar(availableChars[lastIndex]);
        }


        //TODO: actually spawn different character based on selection
    }


    public void togglePlayerTeam(GameObject player, GameObject toggleButton)
    {
        var slotGO = toggleButton.transform.parent;
        var slot = slotGO.GetComponent<PlayerSlotMenuDisplay>();

        // only change on own button 
        if (slotGO.GetSiblingIndex() == teams.getPlayerId(player))
        {
            teams.moveTeam(player);
            slot.setCol(teams.getColorOf(player));
        }

        // everyone can change bot
        if(slot.isBot)
        {
            var bot = teams.getPlayerByID(slotGO.GetSiblingIndex()); //possibly instable, prone to bugs ?!?!?!

            teams.moveTeam(bot);
            slot.setCol(teams.getColorOf(bot));
        }
    }


    public void toggleMap()
    {
        gameState.toggleMap();
    }




    public void playerJoined(Transform playerCursor, bool isBot = false)
    {
        if (inputPrompt.active)
        {
            inputPrompt.SetActive(false);
            botPrompt.SetActive(true);
        }

        playerCursor.parent = cursorParent;
        playerCursor.localPosition = Vector3.zero;

        var slot = Instantiate(playerSlotPrefab, transform.position, Quaternion.identity).transform;
        slot.parent = charSlotParent;
        slot.GetComponent<PlayerSlotMenuDisplay>().setSlot(availableChars[0], teams.getColorOf(playerCursor.gameObject), isBot);
    }

    public void Play()
    {
        gameState.state = GameStateManager.GameState.Ingame;
        LoadMap();
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(FindObjectOfType<GameStateManager>().currentMapInd);
    }

}
