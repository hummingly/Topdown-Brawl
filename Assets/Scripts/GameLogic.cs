using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;

public class GameLogic : MonoBehaviour
{
    private static GameLogic instance;
    private UIManager uiManager;
    private MatchData matchData;
    public static GameLogic Instance { get { return instance; } }

    [SerializeField] private float startGameplayAnimDur = 1;
    [SerializeField] private float spawnBeforeAnimDone = 0.2f;
    private bool roundRunning;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        SceneManager.sceneLoaded += SceneLoadeded;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        matchData = FindObjectOfType<MatchData>();
    }

    void Update()
    {
        if (roundRunning) // bases are initialized
        {
            //if bigger than gamemode max then won
            if (matchData.Score.OnTeamWon())
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
        if (FindObjectOfType<GameStateManager>().State == GameStateManager.GameState.Ingame) //(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1")
        {
            StartCoroutine(InitGameplay());
        }
    }

    public IEnumerator InitGameplay()
    {
        uiManager = FindObjectOfType<UIManager>();

        var mapSize = GameObject.FindGameObjectWithTag("MapBounds").transform.localScale.x;

        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().enabled = false;
        Camera.main.DOOrthoSize(10 * mapSize, startGameplayAnimDur).SetEase(Ease.OutCubic);

        //TODO: Display a message like "GO" (and potentially a countdown?)

        yield return new WaitForSeconds(startGameplayAnimDur - spawnBeforeAnimDone);

        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().enabled = true;

        print("init players");
        matchData.TeamManager.InitPlayers();
        if (matchData.Score.WinCondition == GameMode.WinCondition.Defense)
        {
            print("init bases");
            DestructibleTeamBlock[] defenseBases = GameObject.FindGameObjectWithTag("DefenseBases").transform.GetComponentsInChildren<DestructibleTeamBlock>();
            matchData.Score.SetDefenseBases(defenseBases, matchData.TeamManager.Count);
        }
        roundRunning = true;
    }

    public void IncreaseScore(GameObject player)
    {
        int team = matchData.TeamManager.FindPlayerTeam(player);
        if (team <= -1) {
            throw new UnityException();
        }
        matchData.Score.IncreaseKillScore(team);
        // display new score in UI
        uiManager.UpdateScores(matchData.Score.TeamKills);
    }

    public void GameOver()
    {
        FindObjectOfType<GameStateManager>().EndGame();
        var playerInput = FindObjectsOfType<PlayerInput>();
        foreach (var p in playerInput) {
            p.SwitchCurrentActionMap("Menu");
        }
        uiManager.SetGameOverUI(matchData.TeamManager.GetTeamName(matchData.Score.GetWinningTeam()));
        Time.timeScale = 0;
    }
}
