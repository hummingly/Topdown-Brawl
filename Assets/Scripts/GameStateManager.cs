using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    // SceneManager was already taken... (?) maybe MapManager, but should also do menu etc

    public int currentMapInd = 1;
    public enum GameState { Menu, Selection, Ingame, Victory };
    public GameState state = GameState.Selection;

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
}
