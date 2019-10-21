using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour // Singleton instead of static, so can change variable in inspector
{

    [System.Serializable]
    class Team //or struct?
    {
        public List<GameObject> players = new List<GameObject>();
        public int points;
    }

    private List<Team> teams = new List<Team>();

    [SerializeField] private Color[] playerColors;
    private bool[] usedColors;

    private GameLogic gameLogic;
    private UIManager uiManager;

    private void Awake()
    {
        usedColors = new bool[playerColors.Length];

        gameLogic = GetComponent<GameLogic>();
        uiManager = FindObjectOfType<UIManager>();

        SceneManager.sceneLoaded += SceneLoadeded;
    }



    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1) 
    {
        uiManager = FindObjectOfType<UIManager>();

        //if (scene.name == "MapNormal1")

        if (teams.Count > 0)
        {
            // disable more joining
            GetComponent<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;


            //join prefabs manually for gameplay (spawn the correct prefabs for selected players)

            var existingPlayer = teams[0].players[0];
            // GET ID OF CONTROLLER

            var newPlayer = FindObjectOfType<PlayerSpawner>().joinPlayer();
            //newPlayer.GetComponent<PlayerInput>().device
            // MANUALLY JOIN TO MANAGER?
            //FindObjectOfType<PlayerInputManager>().join(newPlayer);
        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        //if(!gameMode.useTeams)
        //    return;


        //if in menu scene do new teams
        //else if gameplay: no new teams, instead just spawn prefab for exising players

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Selection")
        {
            // first just add all to a new team
            newTeam(player.gameObject);

            FindObjectOfType<MenuManager>().playerJoined(player.transform);

            //TODO: check which player? write string P1 for example
        }
        else
        {
            // in gameplay, but no teams made yet (so just fast testing from 1 scene in editor)

            // for testing add to new team each
            newTeam(player.gameObject);

            FindObjectOfType<PlayerSpawner>().playerJoined(player.transform);
        }
    }

    public void newTeam(GameObject player)
    {
        Team newTeam = new Team();
        newTeam.players.Add(player);
        teams.Add(newTeam);
        //TODO: random color
    }

    public int getTeamOf(GameObject player) //for now jsut 0 or 1
    {
        //return Random.Range(0, 2);

        for(int i = 0; i < teams.Count; i++)
        {
            //int i = team.players.FindIndex(o => o == player);
            int index = teams[i].players.IndexOf(player);

            print(index);

            if (index != -1)
                return i;
        }

        return -1; //error, maybe throw ex
    }

    public void increaseScore(GameObject player) //has to be called if a bullet made a kill (keep track which player shot bullet)
    {
        //find team that GO is in and add point
        int team = getTeamOf(player);

        // if -1 then couldn't find a team/ bot did kill
        if(team != -1)
            teams[team].points++;

        // display new score in UI
        uiManager.updateScore(team);

        //if bigger than gamemode max then won
        if (teams[team].points >= gameLogic.getPointsToWin())
        {
            gameLogic.gameOver();
        }
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
        for(int i = 0; i < usedColors.Length; i++)
            if(!usedColors[i])
            {
                usedColors[i] = true;
                return playerColors[i];
            }

        return Color.black;
    }
}
