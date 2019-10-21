using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance = null;

    [Serializable] class Team //or struct?
    {
        public List<GameObject> players = new List<GameObject>();
        public int points;
    }

    [SerializeField] private Color[] playerColors;

    private bool[] usedColors;

    private List<Team> teams = new List<Team>();


    public GameMode gameMode = new GameMode();
    private UIManager uiManager;
    private PlayerSpawner playerSpawner;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);

        // prepare gamemode (scritpable obj?)

        gameMode.useTeams = true;

        uiManager = GetComponent<UIManager>();

        usedColors = new bool[playerColors.Length];

        SceneManager.sceneLoaded += SceneLoadeded;
    }

    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        if(scene.name == "MapNormal1")
        {
            playerSpawner = FindObjectOfType<PlayerSpawner>();
            playerSpawner.joinplayer();
        }
    }
     



    void Update()
    {
        
    }

    public void increaseScore(GameObject player) //has to be called if a bullet made a kill (keep track which player shot bullet)
    {
        //find team that GO is in and add point
        int team = getTeamOf(player);
        teams[team].points++;
        // display new score in UI
        uiManager.updateScore(team);
        //if bigger than gamemode max then won
        if (teams[team].points >= gameMode.pointsToWin)
        {
            uiManager.setGameOver();
        }
    }


    void OnPlayerJoined(PlayerInput player)
    {
        //player.transform.parent = cursorParent;  //TODO 
        player.transform.localPosition = Vector3.zero;

        print("called");

        //TODO: check if gameplay, no new team, instead just spawn prefab for exising players

        if (gameMode.useTeams)
        {
            // first just add all to a new team
            Team newTeam = new Team();
            newTeam.players.Add(player.gameObject);
            teams.Add(newTeam);
            //TODO: random color
        }

        //TODO: check which player? write string P1 for example

    }


    public int getTeamOf(GameObject player) //for now jsut 0 or 1
    {
        print(teams.Count);
        //return Random.Range(0, 2);
        foreach (Team team in teams)
        {
            int i = team.players.FindIndex(o => o == player);
            print(i);
        }
        return 0;
    }

    public int getScore(int team)
    {
        return teams[team].points;
    }






    public Color getRandUnusedColor()
    {
        var randInd = new int[usedColors.Length];
        for (int i = 0; i < randInd.Length; i++)
            randInd[i] = i;

        randInd = ExtensionMethods.shuffle(randInd);

        for (int i = 0; i < usedColors.Length; i++)
            if (!usedColors[randInd[i]])
            {
                usedColors[randInd[i]] = true;
                return playerColors[randInd[i]];
            }

        return Color.black;
    }

    public Color getUnusedColor()
    {
        for (int i = 0; i < usedColors.Length; i++)
            if (!usedColors[i])
            {
                usedColors[i] = true;
                return playerColors[i];
            }

        return Color.black;
    }
}
