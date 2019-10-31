﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;

public class GameLogic : MonoBehaviour
{
    private static GameLogic instance;
    public static GameLogic Instance { get { return instance; } }


    public GameMode gameMode;

    private UIManager uiManager;
    private TeamManager teams;

    private float mapSize;

    [SerializeField] private float startGameplayAnimDur = 1;
    [SerializeField] private float spawnBeforeAnimDone = 0.2f;


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);     
        else
            instance = this; 

        teams = GetComponent<TeamManager>();

        SceneManager.sceneLoaded += SceneLoadeded;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //gameMode.useTeams = true;
        gameMode.pointsToWin = 8;
    }



    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        // Regularly loaded into gameplay from character selection
        if (FindObjectOfType<GameStateManager>().state == GameStateManager.GameState.Ingame) //(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1")
        {
            StartCoroutine(initGameplay());
        }
    }

    private IEnumerator initGameplay()
    {
        uiManager = FindObjectOfType<UIManager>();

        mapSize = GameObject.FindGameObjectWithTag("MapBounds").transform.localScale.x;



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


        teams.initPlayers();
    }


    public void increaseScore(GameObject player)
    {
        teams.increaseScore(player);

        // display new score in UI
        uiManager.updateScores();

        //if bigger than gamemode max then won
        if (teams.someTeamWon(gameMode.pointsToWin))
        {
            gameOver();
        }
    }

    public void gameOver()
    {
        uiManager.setGameOverUI();
        Time.timeScale = 0;
    }
}
