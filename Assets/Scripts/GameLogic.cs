using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameLogic : MonoBehaviour
{
    private UIManager uiManager;
    private TeamManager teamManager;
    private WinManager winManager;

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
        if (roundRunning)
        {
            if (winManager.OnTeamWon())
            {
                StartRoundEnd();
            }
        }
    }

    public void SetDeathEvent(Vector2 pos)
    {
        if (roundRunning)
        {
            lastDeath = pos;
        }
    }

    public void Kill()
    {
        SceneManager.sceneLoaded -= SceneLoadeded;
        Destroy(gameObject);
    }

    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        Debug.Log("Scene Loadeed!");
        // Regularly loaded into gameplay from character selection
        if (FindObjectOfType<GameStateManager>().State != GameStateManager.GameState.Ingame)
        {
            return;
        }

        uiManager = FindObjectOfType<UIManager>();

        if (winManager.WinCondition != GameMode.WinCondition.Defense)
        {
            Destroy(GameObject.FindGameObjectWithTag("DefenseBases"));
        }

        teamManager.InitPlayers();

        foreach (PlayerMovement p in FindObjectsOfType<PlayerMovement>())
        {
            p.enabled = false;
            p.GetComponentInChildren<PlayerVisuals>().Hide(true);
        }
        foreach (Skill s in FindObjectsOfType<Skill>())
        {
            s.enabled = false;
        }

        if (winManager.WinCondition == GameMode.WinCondition.Defense)
        {
            winManager.InitDefenseBases();
        }
        teamManager.ColorSpawns();

        roundRunning = true;

        if (!GetComponent<TeamManager>().debugFastJoin)
        {
            FindObjectOfType<EffectManager>().StartSequence();
        }
    }

    public void IncreaseScore(GameObject player)
    {
        winManager.IncreaseKillScore(teamManager.FindPlayerTeam(player));
        uiManager.UpdateScores();
    }

    public void StartRoundEnd()
    {
        roundRunning = false;
        float dur = FindObjectOfType<EffectManager>().GameOver(lastDeath);
        if (winManager.GetWinningTeam() > -1)
        {
            StartCoroutine(GameOverUi(dur));
        }
        else
        {
            StartCoroutine(RestartMatchUi(dur));
        }
    }

    private IEnumerator RestartMatchUi(float t)
    {
        var seconds = Mathf.Max(t, 10.0f);
        yield return new WaitForSecondsRealtime(0.5f);
        uiManager.ShowRoundMatchUi();
        yield return new WaitForSecondsRealtime(seconds);

        var dur = FindObjectOfType<EffectManager>().Restart();
        Sequence seqCam = DOTween.Sequence();
        seqCam.AppendInterval(dur);
        seqCam.AppendCallback(() => FindObjectOfType<GameStateManager>().Replay());
    }

    private IEnumerator GameOverUi(float t)
    {
        yield return new WaitForSecondsRealtime(t);
        uiManager.SetGameOverUI();
    }
}
