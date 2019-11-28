using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    // SceneManager was already taken... (?) maybe MapManager, but should also do menu etc
    public enum GameState { Menu, Selection, Ingame, Victory };
    public GameState state = GameState.Selection;
    private TeamManager teams;

    private void Awake()
    {
        teams = GetComponent<TeamManager>();
    }

    public void Play(string map)
    {
        FindObjectOfType<UnityEngine.InputSystem.PlayerInputManager>().joinBehavior = UnityEngine.InputSystem.PlayerJoinBehavior.JoinPlayersManually;
        state = GameState.Ingame;
        SceneManager.LoadScene(map);
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
