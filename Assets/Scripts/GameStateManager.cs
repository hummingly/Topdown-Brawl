using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    // SceneManager was already taken... (?) maybe MapManager, but should also do menu etc

    public int currentMapInd = 1;
    public enum GameState { Start, MatchMaking, Ingame, Pause, End };
    private GameState _state = GameState.MatchMaking;
    public GameState State => _state;

    [SerializeField] private Vector2Int mapRange; //currently maps between 1 and 3
    [SerializeField] private Sprite[] mapSprites;
    [SerializeField] private Image mapImg;

    [SerializeField] private GameMode[] allGameModes;
    private int currentGameModeIndex;

    void Start()
    {
        mapImg.sprite = mapSprites[currentMapInd - mapRange.x];
        currentGameModeIndex = 0;
    }

    void Update()
    {
        if (GetCurrentGameMode().name.Equals("Defense"))
        {
            // hardcoded BAAAD
            currentMapInd = 2;
            mapImg.sprite = mapSprites[currentMapInd - mapRange.x];
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

        mapImg.sprite = mapSprites[currentMapInd - mapRange.x];

        // TODO: resize of somehow fit the img? or all same ratio...
    }

    public string ToggleGameMode()
    {
        currentGameModeIndex = (currentGameModeIndex + 1) % allGameModes.Length;
        return allGameModes[currentGameModeIndex].name;
    }

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
    public void Play() {
        if (State != GameState.MatchMaking) {
            throw new UnityException();
        }
        _state = GameState.Ingame;
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
