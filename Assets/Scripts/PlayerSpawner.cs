using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using DG.Tweening;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;

    [SerializeField] private Transform[] spawnAreas;
    [SerializeField] private float respawnTime = 3;
    [SerializeField] private float zoomBefore = 0.3f;
    [SerializeField] private float stayAfterDeath = 0.3f;
    [SerializeField] private float spawnPosBlockTime = 0.3f;

    [SerializeField] private float playerCameraRadius = 1f;

    private GameLogic gameLogic;
    private TeamManager teams;
    private EffectManager effectManager;
    private CinemachineTargetGroup camTargetGroup;
    //private List<float> spawnTimers = new List<float>();
    [SerializeField] private List<List<float>> spawnPosTimers = new List<List<float>>();

    void Awake()
    {
        gameLogic = FindObjectOfType<GameLogic>();
        teams = FindObjectOfType<TeamManager>();
        effectManager = GetComponent<EffectManager>();
        camTargetGroup = FindObjectOfType<CinemachineTargetGroup>();


        for (int i = 0; i < teams.teams.Count; i++)
        {
            //spawnPosTimers.Add(new List<float>(teams.teams[i].players.Count));

            // shouldn't be necessary?
            var list = new List<float>();
            for (int j = 0; j < teams.teams[i].Count; j++)
                list.Add(0); 
            spawnPosTimers.Add(list);
        }
    }

    void Update()
    {
        foreach(List<float> timers in spawnPosTimers)
        {
            for(int i = 0; i < timers.Count; i++)
            {
                timers[i] -= Time.deltaTime;
            }
        }
    }





    public void PlayerDied(PlayerStats player)
    {
        StartCoroutine(Respawn(player));
    }

    // For joining player manually from preselection
    public GameObject CreatePlayer()
    {
        var player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity).transform;
        
        return player.gameObject;
    }
    public GameObject CreateBot()
    {
        var player = Instantiate(botPrefab, Vector2.zero, Quaternion.identity).transform;

        return player.gameObject;
    }

    // For normal debug playign from gameplay scene
    public void PlayerJoined(Transform player)//void OnPlayerJoined(PlayerInput player)
    {
        SpawnPlayer(player.transform);
    }

    





    IEnumerator Respawn(PlayerStats player)
    {
        SetPlayerActive(false, player);
        //player.GetComponent<Rigidbody2D>().reset(); // TODO: reset movement -> rb.velocity / angularVelocity and player inputDir(?)
        camTargetGroup.RemoveMember(player.transform);


        //after player dying still keep a palceholder at that position for a sec, so that doesn't suddenly change (bad hack: also move that placehodler to camera pos for smooth blend)
        var placeholder = new GameObject().transform;
        placeholder.position = player.transform.position;
        camTargetGroup.AddMember(placeholder, 1, playerCameraRadius); 

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(stayAfterDeath);
        seq.Append(placeholder.DOMove(Camera.main.transform.position, stayAfterDeath/2));
        seq.AppendCallback(() => camTargetGroup.RemoveMember(placeholder));
        //TODO: add ease

        yield return new WaitForSeconds(stayAfterDeath);
        //camTargetGroup.RemoveMember(placeholder);




        // Shortly before player spawns already add a placeholder, so the camera can zoom out & show respawn (bad hack: also move that placehodler to spawn pos for smooth blend)
        yield return new WaitForSeconds(respawnTime - stayAfterDeath - zoomBefore);
        var spawnPos = GetSpawnArea(player.transform);
        placeholder = new GameObject().transform;
        //placeholder.position = spawnPos;
        placeholder.position = Camera.main.transform.position;
        camTargetGroup.AddMember(placeholder, 1, playerCameraRadius);

        Sequence seq2 = DOTween.Sequence();
        seq2.Append(placeholder.DOMove(spawnPos, zoomBefore));
        seq2.AppendCallback(() => camTargetGroup.RemoveMember(placeholder));
        //TODO: add ease

        yield return new WaitForSeconds(zoomBefore);
        //camTargetGroup.RemoveMember(placeholder);

        SetPlayerActive(true, player);
        player.IncreaseHealth(int.MaxValue);
        player.SetInvincible(spawnPos);
        SpawnPlayer(player.transform, spawnPos);
    }

    private void SpawnPlayer(Transform player)
    {
        SpawnPlayer(player, GetSpawnArea(player));
    }

    private void SpawnPlayer(Transform player, Vector2 pos)
    {
        player.position = pos;
        camTargetGroup.AddMember(player.transform, 1, playerCameraRadius);

        effectManager.SquareParticle(player.position);
    }

    private void SetPlayerActive(bool b, PlayerStats player)
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


    private Vector2 GetSpawnArea(Transform player)
    {
        int team = teams.FindPlayerTeam(player.gameObject);


        // Get an open spawn point
        int pos = 0;
        var timers = spawnPosTimers[team];

        for (int i = 0; i < timers.Count; i++)
        {
            if (timers[i] <= 0)
            {
                pos = i;
                timers[i] = spawnPosBlockTime;
                break;
            }
        }


        // Spawn in one of the premade points in the right spawn zone
        //return spawnAreas[team].GetChild(Random.Range(0, spawnAreas[0].childCount)).position;
        if (spawnAreas[team].childCount > pos)
            return spawnAreas[team].GetChild(pos).position;
        else
            return spawnAreas[team].GetChild(Random.Range(0, spawnAreas[team].childCount)).position; //team technically too big for the spawn poitns (eg 4 ppl, but only 3 spots) TODO: additional spots, for now random
    }

    private int GetPlayerTeam() // 0 or 1 for now
    {
        return Random.Range(0, 2);
    }

    public GameObject getSpawnArea(int i)
    {
        return spawnAreas[i].gameObject;
    }
}
