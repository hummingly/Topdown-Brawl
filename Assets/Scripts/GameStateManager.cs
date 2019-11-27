using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    // SceneManager was already taken... (?) maybe MapManager, but should also do menu etc
    public enum GameState { Start, MatchMaking, Ingame, Pause, End };
    private GameState _state = GameState.MatchMaking;
    public GameState State => _state;

    // Loads Start Scene.
    public void Restart() {
        // Game must be paused first.
        if (State == GameState.Ingame) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.Start;
        // TODO: Add Start Screen.
        // SceneManager.LoadScene("Start");
    }

    // Loads Match Making Scene.
    public void MakeMatch() {
        if (State != GameState.Start) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.MatchMaking;
        SceneManager.LoadScene("Selection");
    }

    // Starts actual game.
    public void Play(string map) {
        if (State != GameState.MatchMaking) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.Ingame;
        // TODO: Move data instead of static gameObject...
        FindObjectOfType<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        // TODO: Timer count down...
        StartCoroutine(LoadSceneAsync(map));
    }

    // Pauses in-game.
    public void Pause() {
        if (State != GameState.Ingame) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.Pause;
        // TODO: Add Pause Overlay.
    }

    public void Resume() {
        if (State != GameState.Ingame) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.Ingame;
    }

    // Shows End game statistics and replay/restart functionality.
    public void EndGame() {
        if (State != GameState.Ingame) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.End;
    }

    // Returns to Match Making Scene with previously selected game mode and
    // team composition.
    public void Replay() {
        if (State != GameState.End) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.MatchMaking;
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        var data = FindObjectOfType<MatchData>();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(data.gameObject, SceneManager.GetSceneByName(scene));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
