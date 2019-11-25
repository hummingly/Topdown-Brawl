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
        SceneManager.LoadScene(map);
    }

    // Pauses in-game.
    public void Pause() {
        if (State != GameState.Ingame) {
            Debug.Log("Invalid State Transition");
            return;
        }
        _state = GameState.Pause;
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
}
