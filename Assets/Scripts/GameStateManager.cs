using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    // SceneManager was already taken... (?) maybe MapManager, but should also do menu etc
    public enum GameState { Start, MatchMaking, Ingame, Replay, End };
    private GameState state = GameState.MatchMaking;
    private TeamManager teams;

    public GameState State => state;

    private void Awake()
    {
        teams = GetComponent<TeamManager>();
    }

    public void Play(string map)
    {
        if (State != GameState.MatchMaking)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        FindObjectOfType<UnityEngine.InputSystem.PlayerInputManager>().joinBehavior = UnityEngine.InputSystem.PlayerJoinBehavior.JoinPlayersManually;
        state = GameState.Ingame;
        SceneManager.LoadScene(map);
    }

    public void RestartMatchMaking()
    {
        //keeping players from gameplay to menu won't work atm because static scripts etc

        FindObjectOfType<GameLogic>().Kill();

        SceneManager.LoadScene("Selection");

        Time.timeScale = 1;
    }

    public bool Replay()
    {
        if (State != GameState.Ingame)
        {
            Debug.Log("Invalid State Transition");
            return false;
        }
        state = GameState.Replay;
         foreach (var b in FindObjectsOfType<BotTest>())
        {
            DontDestroyOnLoad(b.gameObject);
        }

        var winManager = FindObjectOfType<WinManager>();
        winManager.ResetRound();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (winManager.WinCondition == GameMode.WinCondition.Defense)
        {
            winManager.InitDefenseBases();
        }
        state = GameState.Ingame;
        Time.timeScale = 1;
        return true;
    }

    public void EndGame() {
        if (State != GameState.Ingame)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        state = GameState.End;
        var playerInput = FindObjectsOfType<PlayerInput>();
        foreach (var p in playerInput)
        {
            p.SwitchCurrentActionMap("Menu");
        }
    }

    IEnumerator LoadSceneAsync(string scene, IEnumerable<GameObject> gameObjects)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        foreach (var o in gameObjects)
        {
            SceneManager.MoveGameObjectToScene(o, SceneManager.GetSceneByName(scene));
        }
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
