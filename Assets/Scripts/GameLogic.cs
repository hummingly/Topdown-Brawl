using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;

public class GameLogic : MonoBehaviour
{
    private static GameLogic instance;
    public static GameLogic Instance { get { return instance; } }

    
    private UIManager uiManager;
    private TeamManager teamManager;
    private WinManager winManager;

    private float mapSize;

    [SerializeField] private float startGameplayAnimDur = 1;
    [SerializeField] private float spawnBeforeAnimDone = 0.2f;

    private bool roundRunning;


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);     
        else
            instance = this;

        teamManager = GetComponent<TeamManager>();

        SceneManager.sceneLoaded += SceneLoadeded;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        winManager = GetComponent<WinManager>();
    }

    void Update()
    {
        //GetComponent<GameStateManager>().state == GameStateManager.GameState.Ingame doesn't help because InitGameplay is Coroutine
        if (roundRunning) // bases are initialized
        {
            //if bigger than gamemode max then won
            if (winManager.OnTeamWon(teamManager.GetTeams()))
            {
                roundRunning = false;
                GameOver();
            }
        }
    }



    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        // Regularly loaded into gameplay from character selection
        if (FindObjectOfType<GameStateManager>().state == GameStateManager.GameState.Ingame) //(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1")
        {
            StartCoroutine(InitGameplay());
        }
    }

    private IEnumerator InitGameplay()
    {
        if (winManager.gameMode.winCondition != GameMode.WinCondition.Defense)
            Destroy(GameObject.FindGameObjectWithTag("DefenseBases"));


        uiManager = FindObjectOfType<UIManager>();

        mapSize = GameObject.FindGameObjectWithTag("MapBounds").transform.localScale.x;

        if (!GetComponent<TeamManager>().debugFastJoin)
        {
            FindObjectOfType<EffectManager>().startSequence();
            //yield return new WaitForSeconds(FindObjectOfType<EffectManager>().startSequence());
        }


        // TODO: put all this shit into a camera script !!!

        // Short cam animation before spawn
        //Camera.main.DOOrthoSize(30, 2);
        // doesnt work since is controlled by target group, so new vcam
        /*var newGO = new GameObject("Zoom Cam");
        newGO.transform.position = new Vector3(0,0, -11);
        Cinemachine.CinemachineVirtualCamera testCam = newGO.AddComponent<Cinemachine.CinemachineVirtualCamera>();
        testCam.MoveToTopOfPrioritySubqueue();
        testCam.transform.DOMove(Vector2.one * 10, 2);
        testCam.m_Lens.OrthographicSize = 50;
        //testCam.do;*/

        // TODO: maybe move camera across the map once from left to center, then zoom out

        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().enabled = false;
        Camera.main.DOOrthoSize(10 * mapSize, startGameplayAnimDur).SetEase(Ease.OutCubic);

        //TODO: Display a message like "GO" (and potentially a countdown?)

        yield return new WaitForSeconds(startGameplayAnimDur - spawnBeforeAnimDone);

        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().enabled = true;


        //print("init players");
        teamManager.InitPlayers();
        if (winManager.gameMode.winCondition == GameMode.WinCondition.Defense)
        {
            //print("init bases");
            GameObject defenseBasesParent = GameObject.FindGameObjectWithTag("DefenseBases");
            teamManager.InitDefenseBases(defenseBasesParent);
        }

        teamManager.colorSpawns();


        roundRunning = true;
    }


    public void IncreaseScore(GameObject player)
    {
        teamManager.IncreaseScore(player);
        // display new score in UI
        uiManager.UpdateScores();
    }

    public void GameOver()
    {
        uiManager.SetGameOverUI();
        Time.timeScale = 0;
    }
}
