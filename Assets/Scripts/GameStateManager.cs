using UnityEngine;
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
            throw new UnityException();
        }
        _state = GameState.Start;
    }

    // Loads Match Making Scene.
    public void MakeMatch() {
        if (State != GameState.Start) {
            throw new UnityException();
        }
        _state = GameState.MatchMaking;
    }

    // Starts actual game.
    public void Play(int map) {
        if (State != GameState.MatchMaking) {
            throw new UnityException();
        }
        _state = GameState.Ingame;
        SceneManager.LoadScene(map);
    }

    // Pauses in-game.
    public void Pause() {
        if (State != GameState.Ingame) {
            throw new UnityException();
        }
        _state = GameState.Pause;
    }

    // Shows End game statistics and replay/restart functionality.
    public void EndGame() {
        if (State != GameState.Ingame) {
            throw new UnityException();
        }
        _state = GameState.End;
    }

    // Returns to Match Making Scene with previously selected game mode and
    // team composition.
    public void Replay() {
        if (State != GameState.End) {
            throw new UnityException();
        }
        _state = GameState.MatchMaking;
    }
}
