using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnAreas;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    // Spawn player 6 blocks above center
    void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = getSpawnArea();
    }

    private Vector2 getSpawnArea()
    {
        int team = getPlayerTeam();

        // Spawn in one of the premade points in the right spawn zone
        return spawnAreas[team].GetChild(Random.Range(0, spawnAreas[0].childCount)).position;
    }

    private int getPlayerTeam() // 0 or 1 for now
    {
        return Random.Range(0, 2);
    }
}
