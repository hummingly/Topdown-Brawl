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
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        teams = GetComponent<TeamManager>();

        DontDestroyOnLoad(gameObject);
    }




    void Start()
    {
        //gameMode.useTeams = true;
        gameMode.pointsToWin = 8;
    }

    void Update()
    {
        
    }

    

    



    public int getPointsToWin()
    {
        return gameMode.pointsToWin;
    }

    public void gameOver()
    {
        FindObjectOfType<UIManager>().setGameOverUI();
        Time.timeScale = 0;
    }


    


}
