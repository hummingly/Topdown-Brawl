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

    [SerializeField] private Vector2Int mapRange = new Vector2Int(1,2); //currently a map at index 1 and at 2
    [SerializeField] private Sprite[] mapSprites;
    [SerializeField] private Image mapImg;

    void Start()
    {
        mapImg.sprite = mapSprites[currentMapInd - mapRange.x];
    }

    void Update()
    {
        
    }

    public void toggleMap()
    {
        currentMapInd++;
        if (currentMapInd > mapRange.y)
            currentMapInd = mapRange.x;

        mapImg.sprite = mapSprites[currentMapInd - mapRange.x];

        // TODO: resize of somehow fit the img? or all same ratio...
    }
}
