using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    // SceneManager was already taken... (?) maybe MapManager, but should also do menu etc

    public int currentMapInd = 1;
    public enum GameState { Menu, Selection, Ingame, Victory };
    public GameState state = GameState.Selection;

    [SerializeField] private Vector2Int mapRange; //currently maps between 1 and 3
    [SerializeField] private Sprite[] mapSprites;

    [SerializeField] private GameMode[] allGameModes;
    private int currentGameModeIndex;
    private MenuManager menu;
    private TeamManager teams;

    [SerializeField] private GameObject playerCursorPrefab;

    private void Awake()
    {
        teams = GetComponent<TeamManager>();
        menu = FindObjectOfType<MenuManager>();

        currentGameModeIndex = 1; //start with defense, cuz focus on team play
    }



    void Update()
    {
        if (state == GameState.Selection && GetCurrentGameMode().name.Equals("Defense"))
        {
            // hardcoded BAAAD
            currentMapInd = 2;
            if (!menu)
            {
                menu = FindObjectOfType<MenuManager>();
            }
            else
            {
                menu.SetMapImg(mapSprites[currentMapInd - mapRange.x]);
            }
        }
    }

    public GameMode GetCurrentGameMode()
    {
        return allGameModes[currentGameModeIndex];
    }

    public void ToggleMap()
    {
        currentMapInd++;
        if (currentMapInd > mapRange.y)
            currentMapInd = mapRange.x;

        menu.SetMapImg(mapSprites[currentMapInd - mapRange.x]);

        // TODO: resize of somehow fit the img? or all same ratio...
    }

    public string ToggleGameMode()
    {
        currentGameModeIndex = (currentGameModeIndex + 1) % allGameModes.Length;
        return allGameModes[currentGameModeIndex].name;
    }

    public void Play()
    {
        teams.SaveCharacters();
        FindObjectOfType<UnityEngine.InputSystem.PlayerInputManager>().joinBehavior = UnityEngine.InputSystem.PlayerJoinBehavior.JoinPlayersManually;
        state = GameState.Ingame;
        LoadMap();
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(FindObjectOfType<GameStateManager>().currentMapInd);
    }

    public void GoToSelection()
    {
        //keeping players from gameplay to menu won't work atm because static scripts etc


        /*state = GameState.Selection;

        SceneManager.LoadScene("Selection");

        FindObjectOfType<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        FindObjectOfType<PlayerInputManager>().playerPrefab = playerCursorPrefab;
        teams.Wipe();

        Time.timeScale = 1;*/


        FindObjectOfType<GameLogic>().Kill();

        SceneManager.LoadScene("Selection");

        Time.timeScale = 1;
    }

    public void Restart()
    {
        BotTest[] bots = FindObjectsOfType<BotTest>();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // keep bots (poor hack)
        for (int i = 0; i < bots.Length; i++)
        {
            //teams.AddBot(i);

            GameObject bot = new GameObject("Empty Bot Cursor");
            //AddToEmptyOrSmallestTeam(bot);
            teams.GetTeams()[teams.FindPlayerTeam(bots[i].gameObject)].ReplacePlayer(bots[i].gameObject, bot);

            DontDestroyOnLoad(bot);
        }

        Time.timeScale = 1;
    }
}
