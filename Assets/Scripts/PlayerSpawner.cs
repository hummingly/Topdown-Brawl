using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;

    [SerializeField] private Transform[] spawnAreas;
    [SerializeField] private float respawnTime = 3;
    [SerializeField] private float zoomBefore = 0.3f;
    [SerializeField] private float stayAfterDeath = 0.3f;

    private GameLogic gameLogic;
    private TeamManager teams;
    private EffectManager effectManager;
    private CinemachineTargetGroup camTargetGroup;
    //private List<float> spawnTimers = new List<float>();

    void Awake()
    {
        gameLogic = FindObjectOfType<GameLogic>();
        teams = FindObjectOfType<TeamManager>();
        effectManager = GetComponent<EffectManager>();
        camTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
    }

    void Update()
    {
        
    }




    public void playerDied(PlayerStats player)
    {
        StartCoroutine(respawn(player));
    }

    // For joining player manually from preselection
    public GameObject spawnPlayer()
    {
        var player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity).transform;

        return player.gameObject;
    }
    public GameObject spawnBot()
    {
        var player = Instantiate(botPrefab, Vector2.zero, Quaternion.identity).transform;

        return player.gameObject;
    }

    // For normal debug playign from gameplay scene
    public void playerJoined(Transform player)//void OnPlayerJoined(PlayerInput player)
    {
        spawnPlayer(player.transform);
    }

    





    IEnumerator respawn(PlayerStats player)
    {
        setPlayerActive(false, player);
        //player.GetComponent<Rigidbody2D>().reset(); // TODO: reset movement -> rb.velocity / angularVelocity and player inputDir(?)
        camTargetGroup.RemoveMember(player.transform);


        //after playering dying still keep a palceholder at that position, so that doesn't suddenly change
        var placeholder = new GameObject().transform;
        placeholder.position = player.transform.position;
        camTargetGroup.AddMember(placeholder, 1, 1);

        yield return new WaitForSeconds(stayAfterDeath);
        camTargetGroup.RemoveMember(placeholder);




        // Shortly before player spawns already add a placeholder, so the camera can zoom out & show respawn
        yield return new WaitForSeconds(respawnTime - stayAfterDeath - zoomBefore);
        var spawnPos = getSpawnArea(player.transform);
        placeholder = new GameObject().transform;
        placeholder.position = spawnPos;
        camTargetGroup.AddMember(placeholder, 1, 1);


        yield return new WaitForSeconds(zoomBefore);
        camTargetGroup.RemoveMember(placeholder);

        setPlayerActive(true, player);
        player.respawned();
        spawnPlayer(player.transform, spawnPos);
    }

    private void spawnPlayer(Transform player)
    {
        spawnPlayer(player, getSpawnArea(player));
    }

    private void spawnPlayer(Transform player, Vector2 pos)
    {
        player.position = pos;
        camTargetGroup.AddMember(player.transform, 1, 1);

        effectManager.squareParticle(player.position);
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


    private Vector2 getSpawnArea(Transform player)
    {
        int team = teams.getTeamOf(player.gameObject);

        // Spawn in one of the premade points in the right spawn zone
        return spawnAreas[team].GetChild(Random.Range(0, spawnAreas[0].childCount)).position;
    }

    private int getPlayerTeam() // 0 or 1 for now
    {
        return Random.Range(0, 2);
    }
}
