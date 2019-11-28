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

    //[SerializeField] private float startGameplayAnimDur = 1;
    //[SerializeField] private float spawnBeforeAnimDone = 0.2f;

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

    public void setDeathEvent(Vector2 pos)
    {
        if(roundRunning)
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
        if (FindObjectOfType<GameStateManager>().state == GameStateManager.GameState.Ingame) //(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1")
        {
            uiManager = FindObjectOfType<UIManager>();


            if (winManager.gameMode.winCondition != GameMode.WinCondition.Defense)
                Destroy(GameObject.FindGameObjectWithTag("DefenseBases"));


            teamManager.InitPlayers();

            foreach (PlayerMovement p in FindObjectsOfType<PlayerMovement>())
            {
                p.enabled = false;
                p.GetComponentInChildren<PlayerVisuals>().Hide(true);
            }

            if (winManager.gameMode.winCondition == GameMode.WinCondition.Defense)
            {
                GameObject defenseBasesParent = GameObject.FindGameObjectWithTag("DefenseBases");
                teamManager.InitDefenseBases(defenseBasesParent);
            }
            teamManager.colorSpawns();

            roundRunning = true;

            if (!GetComponent<TeamManager>().debugFastJoin)
                FindObjectOfType<EffectManager>().startSequence();
        }
    }




    public void IncreaseScore(GameObject player)
    {
        teamManager.IncreaseScore(player);
        // display new score in UI
        uiManager.UpdateScores();
    }

    public void GameOver()
    {
        float dur = FindObjectOfType<EffectManager>().gameOver(lastDeath);
        StartCoroutine(showGameOverUi(dur));
    }

    private IEnumerator showGameOverUi(float t)
    {
        yield return new WaitForSecondsRealtime(t);

        uiManager.SetGameOverUI();
    }
}
