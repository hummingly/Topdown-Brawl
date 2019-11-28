using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    // SceneManager was already taken... (?) maybe MapManager, but should also do menu etc
    public enum GameState { Start, MatchMaking, Ingame, Pause, End };
    private GameState state = GameState.Start;
    public GameState State => state;

    private void OnPlayerJoined(PlayerInput player)
    {
        var teamManager = FindObjectOfType<MatchData>().TeamManager;
        Debug.Log("Player joined");
        //if in menu scene do new teams
        //else if gameplay: no new teams, instead just spawn prefab for exising players
        if (SceneManager.GetActiveScene().name == "Selection")
        {
            // first just add all to a new team
            teamManager.AddPlayerInput(player);
            //TODO: check which player? write string P1 for example
            return;
        }

        //else if (teams.Count <= 1)// FOR SOME REASON still got called even when coming from scene
        if (SceneManager.GetActiveScene().name == "gameplayDEV")
        {
            // in gameplay, but no teams made yet (so just fast testing from 1 scene in editor)

            // for testing add to a new team each new player
            if (teamManager.AddToSmallestTeam(player.gameObject))
            {
                FindObjectOfType<PlayerSpawner>().PlayerJoined(player.transform);
                player.GetComponentInChildren<PlayerVisuals>().InitColor(teamManager.GetColorOf(player.gameObject));
            }
        }
    }

    // Loads Start Scene.
    public void Restart()
    {
        // Game must be paused first.
        if (State == GameState.Ingame)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        state = GameState.Start;
        SceneManager.LoadScene("Start");
    }

    // Loads Match Making Scene.
    public void MakeMatch()
    {
        if (State != GameState.Start)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        state = GameState.MatchMaking;
        var playerInputs = FindObjectsOfType<PlayerInput>();
        foreach (var p in playerInputs)
        {
            Destroy(p);
        }
        StartCoroutine(LoadSceneAsync("Selection", gameObject));
    }

    // Starts actual game.
    public void Play(string map)
    {
        if (State != GameState.MatchMaking)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        FindObjectOfType<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        state = GameState.Ingame;
        // TODO: Timer count down..
        var list = new GameObject[] { FindObjectOfType<MatchData>().gameObject, gameObject };
        StartCoroutine(LoadSceneAsync(map, list));
    }

    // Pauses in-game.
    public void Pause()
    {
        if (State != GameState.Ingame)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        state = GameState.Pause;
        // TODO: Add Pause Overlay.
    }

    public void Resume()
    {
        if (State != GameState.Ingame)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        state = GameState.Ingame;
    }

    // Shows End game statistics and replay/restart functionality.
    public void EndGame()
    {
        if (State != GameState.Ingame)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        state = GameState.End;
    }

    // Returns to Match Making Scene with previously selected game mode and
    // team composition.
    public void Replay()
    {
        if (State != GameState.End)
        {
            Debug.Log("Invalid State Transition");
            return;
        }
        state = GameState.MatchMaking;
        var list = new GameObject[] { FindObjectOfType<MatchData>().gameObject, gameObject };
        StartCoroutine(LoadSceneAsync("Selection", list));
    }

    IEnumerator LoadSceneAsync(string scene, GameObject gameObject)
    {
        return LoadSceneAsync(scene, new GameObject[] { gameObject });
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
