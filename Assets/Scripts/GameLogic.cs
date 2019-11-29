using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;

public class GameLogic : MonoBehaviour
{
    //private static GameLogic instance;
    //public static GameLogic Instance { get { return instance; } }

    private UIManager uiManager;
    private TeamManager teamManager;
    private WinManager winManager;

    private float mapSize;

    [SerializeField] private float startGameplayAnimDur = 1;
    [SerializeField] private float spawnBeforeAnimDone = 0.2f;

    private bool roundRunning;

    private Vector2 lastDeath;

    private void Awake()
    {
        /*if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;*/

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
        if (roundRunning) // bases are initialized
        {
            //if bigger than gamemode max then won
            if (winManager.OnTeamWon())
            {
                if (winManager.GetWinningTeam() == -1)
                {
                    RestartOver();
                    FindObjectOfType<GameStateManager>().RestartMatch();
                }
                else
                {
                    roundRunning = false;
                    GameOver();
                }
            }
        }
    }

    public void SetDeathEvent(Vector2 pos)
    {
        if (roundRunning)
            lastDeath = pos;
    }

    public void Kill()
    {
        SceneManager.sceneLoaded -= SceneLoadeded;
        Destroy(gameObject);
    }

    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        // Regularly loaded into gameplay from character selection
        if (FindObjectOfType<GameStateManager>().State == GameStateManager.GameState.Ingame) //(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1")
        {
            StartCoroutine(InitGameplay());
        }
    }

    private IEnumerator InitGameplay()
    {
        if (winManager.WinCondition != GameMode.WinCondition.Defense)
            Destroy(GameObject.FindGameObjectWithTag("DefenseBases"));

        uiManager = FindObjectOfType<UIManager>();

        mapSize = GameObject.FindGameObjectWithTag("MapBounds").transform.localScale.x;

        if (!GetComponent<TeamManager>().debugFastJoin)
        {
            FindObjectOfType<EffectManager>().StartSequence();
        }

        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().enabled = false;
        Camera.main.DOOrthoSize(10 * mapSize, startGameplayAnimDur).SetEase(Ease.OutCubic);

        //TODO: Display a message like "GO" (and potentially a countdown?)

        yield return new WaitForSeconds(startGameplayAnimDur - spawnBeforeAnimDone);

        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().enabled = true;

        teamManager.InitPlayers();
        if (winManager.WinCondition == GameMode.WinCondition.Defense)
        {
            winManager.InitDefenseBases();
        }
        teamManager.ColorSpawns();
        roundRunning = true;
    }

    public void IncreaseScore(GameObject player)
    {
        winManager.IncreaseKillScore(teamManager.FindPlayerTeam(player));
        // display new score in UI
        uiManager.UpdateScores();
    }

    public void RestartOver()
    {
        float dur = FindObjectOfType<EffectManager>().GameOver(lastDeath);
        StartCoroutine(RoundOverUi(dur));
    }

    // TODO: Create actual Ui
    private IEnumerator RoundOverUi(float t)
    {
        yield return new WaitForSecondsRealtime(t);
    }

    public void GameOver()
    {
        float dur = FindObjectOfType<EffectManager>().GameOver(lastDeath);
        StartCoroutine(ShowGameOverUi(dur));
    }

    private IEnumerator ShowGameOverUi(float t)
    {
        yield return new WaitForSecondsRealtime(t);

        uiManager.SetGameOverUI();
    }
}
