using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnAreas;
    [SerializeField] private float respawnTimer = 3;

    private CinemachineTargetGroup camTargetGroup;
    //private List<float> spawnTimers = new List<float>();

    void Start()
    {
        camTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
    }

    void Update()
    {
        
    }




    public void playerDied(PlayerStats player)
    {
        StartCoroutine(respawn(player));
    }
    void OnPlayerJoined(PlayerInput player)
    {
        spawnPlayer(player.transform);
        camTargetGroup.AddMember(player.transform, 1, 1);
    }


    IEnumerator respawn(PlayerStats player)
    {
        setPlayerActive(false, player);
        //player.GetComponent<Rigidbody2D>().reset(); // TODO: reset movement -> rb.velocity / angularVelocity and player inputDir(?)

        yield return new WaitForSeconds(respawnTimer);

        setPlayerActive(true, player);
        player.IncreaseHealth(int.MaxValue);
        spawnPlayer(player.transform);
    }

    private void spawnPlayer(Transform player)
    {
        player.position = getSpawnArea();
    }

    private void setPlayerActive(bool b, PlayerStats player)
    {
        //player.gameObject.SetActive(b);

        player.GetComponent<Collider2D>().enabled = b;

        foreach (MonoBehaviour m in player.GetComponents<MonoBehaviour>())
            if (m is PlayerInput == false)
                m.enabled = b;

        foreach (SpriteRenderer s in player.GetComponentsInChildren<SpriteRenderer>())
                s.enabled = b;

        player.GetComponentInChildren<SpriteMask>().enabled = b;
        player.GetComponentInChildren<Canvas>().enabled = b;
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
