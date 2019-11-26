using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private int currentGameModeIndex = 0;
    [SerializeField] private int currentMapIndex = 1;
    private TeamManager teamManager;
    public List<Character> availableChars = new List<Character>();

    // UI Data
    [SerializeField] private GameMode[] gameModes;
    [SerializeField] private string[] maps;

    // UI Elements
    [SerializeField] private GameObject inputPrompt;
    [SerializeField] private GameObject botPrompt;
    [SerializeField] private Transform charSlotParent;
    [SerializeField] private Transform cursorParent;
    [SerializeField] private GameObject playerSlotPrefab;
    [SerializeField] private Sprite[] mapSprites;
    [SerializeField] private Image mapImg;
    [SerializeField] private TextMeshProUGUI gameModeText;

    private GameMode SelectedGameMode => gameModes[currentGameModeIndex];
    private String SelectedMap => maps[currentMapIndex];

    void Awake()
    {
        teamManager = FindObjectOfType<TeamManager>();
    }

    private void Start() {
        // TODO: When data is restructed and maps finished, remove this.
        UpdateMapUi();
        UpdateGameModeUi();
    }

    void Update()
    {
        if (IsReady()) {
            Play();
        }
    }

    private bool IsReady() {
        PlayerSlotMenuDisplay[] slots = charSlotParent.GetComponentsInChildren<PlayerSlotMenuDisplay>();
        if (slots.Length == 0) {
            return false;
        }

        return Array.TrueForAll(slots, s => s.transform.GetComponentInChildren<Toggle>().isOn);
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
        if (slot.myPlayer == player || slot.isBot)
        {
            var lastIndex = availableChars.IndexOf(slot.chara);
            lastIndex += dir;
            if (lastIndex < 0)
            {
                lastIndex = availableChars.Count - 1;
            }

            if (lastIndex >= availableChars.Count)
            {
                lastIndex = 0;
            }

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
            teamManager.MoveTeam(player);
            slot.SetCol(teamManager.GetColorOf(player));
            slot.myPlayer.GetComponent<MenuCursor>().SetColor(teamManager.GetColorOf(player));
            return;
        }

        // everyone can change bot
        if (slot.isBot)
        {
            var bot = slot.myPlayer;//teams.getPlayerByID(getSlotInd(slotGO)); //possibly instable, prone to bugs ?!?!?!

            // Instead of going through each team go through all colors from when bot placed, and then delete
            slot.botDeleteCounter++;

            // Already max team, so remove instead (after cycle through all colors)
            if (slot.botDeleteCounter >= teamManager.Count)
            {
                teamManager.Remove(bot);//remove bot cursor in team list

                //replace the slot with an empty fill one
                var index = slot.transform.GetSiblingIndex();
                Destroy(slot.gameObject);
                Transform empty = new GameObject("Fill", typeof(RectTransform)).transform;
                empty.SetParent(charSlotParent, false);
                empty.SetSiblingIndex(index);

                Destroy(bot);//remove bot cursor

                Debug.Log("Removed bot from selection");
            }
            else
            {
                teamManager.MoveTeam(bot);
                slot.SetCol(teamManager.GetColorOf(bot));
            }
        }
    }

    public void ToggleMap()
    {
        if (SelectedGameMode.name.Equals("Defense"))
        {
            // TODO: hardcoded BAAAD
            return;
        }
        currentMapIndex = NextIndex(currentMapIndex, maps.Length);
        UpdateMapUi();
    }

    public void ToggleGameMode(GameObject button)
    {
        currentGameModeIndex = NextIndex(currentGameModeIndex, gameModes.Length);
        UpdateGameModeUi();
        if (SelectedGameMode.name.Equals("Defense"))
        {
            // TODO: hardcoded BAAAD
            currentMapIndex = 1;
            UpdateMapUi();
        }
    }

    // This is only called when a player or bot has been added successfully by
    // the AddToSmallestTeam method in TeamManager.
    // TODO: Move this to add logic somehow
    public void PlayerJoined(Transform playerCursor, bool isBot = false)
    {
        if (inputPrompt.activeInHierarchy)
        {
            inputPrompt.SetActive(false);
            botPrompt.SetActive(true);
        }

        if (!isBot)
        {
            // Attach cursor to scene and place in the middle of the screen.
            playerCursor.SetParent(cursorParent);
            playerCursor.localPosition = Vector3.zero;
            playerCursor.GetComponent<MenuCursor>().Setup(teamManager.playerNrs.IndexOf(playerCursor.gameObject), teamManager.GetColorOf(playerCursor.gameObject));
        }

        var slot = Instantiate(playerSlotPrefab, transform.position, Quaternion.identity).transform;
        slot.SetParent(charSlotParent);
        slot.GetComponent<PlayerSlotMenuDisplay>().SetSlot(playerCursor, availableChars[0], teamManager.GetColorOf(playerCursor.gameObject), isBot, teamManager.playerNrs.IndexOf(playerCursor.gameObject));
        slot.transform.localScale = Vector3.one;
        foreach (Transform child in charSlotParent)
        {
            if (child.name == "Fill")
            {
                var index = child.transform.GetSiblingIndex();
                Destroy(child.gameObject);
                slot.transform.SetSiblingIndex(index);
                return;
            }
        }
        slot.transform.SetSiblingIndex(charSlotParent.childCount);
    }

    private void Play()
    {
        teamManager.SaveCharacters(this);
        FindObjectOfType<GameStateManager>().Play(SelectedMap);
    }

    public Character GetCharacterOfPlayer(GameObject player)
    {
        PlayerSlotMenuDisplay[] characters = charSlotParent.GetComponentsInChildren<PlayerSlotMenuDisplay>();
        foreach (PlayerSlotMenuDisplay character in characters)
        {
            if (character != null && character.myPlayer == player)
            {
                return character.chara;
            }
        }
        return null;
    }

    private void UpdateGameModeUi()
    {
        gameModeText.SetText(SelectedGameMode.name);
        FindObjectOfType<WinManager>().SetGameMode(SelectedGameMode, teamManager.Count);
    }

    private void UpdateMapUi()
    {
        mapImg.sprite = mapSprites[currentMapIndex];
    }

    private int NextIndex(int index, int max)
    {
        if (index >= max - 1)
        {
            return 0;
        }
        return index + 1;
    }
}
