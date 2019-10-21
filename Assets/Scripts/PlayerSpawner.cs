using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform[] spawnAreas;
    [SerializeField] private float respawnTimer = 3;
    [SerializeField] private float zoomBefore = 0.3f;

    private GameLogic gameLogic;
    private EffectManager effectManager;
    private CinemachineTargetGroup camTargetGroup;
    //private List<float> spawnTimers = new List<float>();

    void Awake()
    {
        gameLogic = FindObjectOfType<GameLogic>();
        effectManager = GetComponent<EffectManager>();
        camTargetGroup = FindObjectOfType<CinemachineTargetGroup>();


    }

    void Start()
    {

    }

    void Update()
    {
        
    }




    public void playerDied(PlayerStats player)
    {
        StartCoroutine(respawn(player));
    }
    public void joinplayer()//void OnPlayerJoined(PlayerInput player)
    {
        var player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity).transform;

        //GameObject.FindObjectOfType<GameLogic>().addPlayer(player.gameObject);
        spawnPlayer(player.transform);
    }


    IEnumerator respawn(PlayerStats player)
    {
        setPlayerActive(false, player);
        //player.GetComponent<Rigidbody2D>().reset(); // TODO: reset movement -> rb.velocity / angularVelocity and player inputDir(?)
        camTargetGroup.RemoveMember(player.transform);

        // Shortly before player spawns already add a placeholder, so the camera can zoom out & show respawn
        yield return new WaitForSeconds(respawnTimer * zoomBefore);
        var spawnPos = getSpawnArea(player.transform);
        var placeholder = new GameObject().transform;
        placeholder.position = spawnPos;
        camTargetGroup.AddMember(placeholder, 1, 1);

        yield return new WaitForSeconds(respawnTimer * (1 - respawnTimer));
        camTargetGroup.RemoveMember(placeholder);

        setPlayerActive(true, player);
        player.IncreaseHealth(int.MaxValue);
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
        int team = 0;//gameLogic.getTeamOf(player.gameObject);

        // Spawn in one of the premade points in the right spawn zone
        return spawnAreas[team].GetChild(Random.Range(0, spawnAreas[0].childCount)).position;
    }


}
