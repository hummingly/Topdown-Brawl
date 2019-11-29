using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private TeamManager teams;

    public List<Character> availableChars = new List<Character>();
    private int currentMapIndex = 1;
    [SerializeField] private int currentGameModeIndex = 1;
    [SerializeField] private string[] maps;
    [SerializeField] private GameMode[] gameModes;

    // UI Elements
    [SerializeField] private GameObject inputPrompt;
    [SerializeField] private GameObject botPrompt;
    [SerializeField] private Transform charSlotParent;
    [SerializeField] private Transform cursorParent;
    [SerializeField] private GameObject playerSlotPrefab;
    [SerializeField] private Sprite[] mapSprites;
    [SerializeField] private Image mapImg;
    [SerializeField] private TextMeshProUGUI gameModeText;

    private string SelectedMap => maps[currentMapIndex];
    private GameMode SelectedGameMode => gameModes[currentGameModeIndex];

    void Awake()
    {
        teams = FindObjectOfType<TeamManager>();
    }

    private void Start()
    {
        UpdateMapUi();
        UpdateGameModeUi();
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
            teams.MoveTeam(player);
            slot.SetCol(teams.GetColorOf(player));
            slot.myPlayer.GetComponent<MenuCursor>().SetColor(teams.GetColorOf(player));
            return;
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
                empty.SetParent(charSlotParent, false);
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
        if (SelectedGameMode.name.Equals("Defense"))
        {
            // hardcoded BAAAD
            currentMapIndex = 1;
            UpdateMapUi();
            return;
        }
        currentMapIndex = NextIndex(currentMapIndex, maps.Length);
        UpdateMapUi();
    }

    public void ToggleGameMode(GameObject button)
    {
        currentGameModeIndex = NextIndex(currentGameModeIndex, gameModes.Length);
        if (SelectedGameMode.name.Equals("Defense"))
        {
            // hardcoded BAAAD
            currentMapIndex = 1;
            UpdateMapUi();
        }
        UpdateGameModeUi();
    }

    public void PlayerJoined(Transform playerCursor, bool isBot = false, int place = 0)
    {
        if (inputPrompt.activeInHierarchy)
        {
            inputPrompt.SetActive(false);
            botPrompt.SetActive(true);
        }

        playerCursor.SetParent(cursorParent, false);
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
        slot.SetParent(charSlotParent);
        slot.GetComponent<PlayerSlotMenuDisplay>().SetSlot(playerCursor, availableChars[0], teams.GetColorOf(playerCursor.gameObject), isBot, teams.playerNrs.IndexOf(playerCursor.gameObject));
        slot.transform.localScale = Vector3.one;

        // for bot
        if (replaced)
        {
            slot.transform.SetSiblingIndex(place);
        }

        //for player on joining if full (but with bots or empty GOs)
        if (!isBot)
        {
            var maxPlayerCount = SelectedGameMode.maxTeams * SelectedGameMode.maxTeamSize;
            if (charSlotParent.childCount >= maxPlayerCount)
            {
                foreach (Transform child in charSlotParent)
                {
                    if (!child.GetComponent<PlayerSlotMenuDisplay>())
                    {
                        var index = child.transform.GetSiblingIndex();
                        Destroy(child.gameObject);
                        slot.transform.SetSiblingIndex(index);
                        return;
                    }
                }
            }
        }
    }

    public void Play()
    {
        SaveCharacters();
        FindObjectOfType<GameStateManager>().Play(SelectedMap);
    }

    public void SaveCharacters()
    {
        PlayerSlotMenuDisplay[] characters = charSlotParent.GetComponentsInChildren<PlayerSlotMenuDisplay>();
        foreach (GameObject p in teams.playerNrs)
        {
            var character = Array.Find(characters, c => c != null && c.myPlayer == p);
            if (character.chara != null)
            {
                teams.playerChars.Add(character.chara);
            }
        }
    }

    private void UpdateGameModeUi()
    {
        gameModeText.SetText(SelectedGameMode.name);
        FindObjectOfType<WinManager>().gameMode = SelectedGameMode;
    }

    public void UpdateMapUi()
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
