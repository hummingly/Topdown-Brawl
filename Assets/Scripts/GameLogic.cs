using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameLogic : MonoBehaviour
{
    private static GameLogic instance;
    public static GameLogic Instance { get { return instance; } }


    public GameMode gameMode;

    private UIManager uiManager;
    private TeamManager teams;


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




    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        // Regularly loaded into gameplay from character selection
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapBig")
        {
            uiManager = FindObjectOfType<UIManager>();
        }
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
