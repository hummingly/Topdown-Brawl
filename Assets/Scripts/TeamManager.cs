using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour // Singleton instead of static, so can change variable in inspector
{

    [System.Serializable]
    public class Team //or struct?
    {
        public List<GameObject> players = new List<GameObject>();
        public int points;
    }

    public List<Team> teams = new List<Team>();
    public List<GameObject> playerIDs = new List<GameObject>();

    [SerializeField] private Color[] teamColors;

    private GameLogic gameLogic;
    private UIManager uiManager;

    private void Awake()
    {
        teamColors = ExtensionMethods.shuffle(teamColors);

        gameLogic = GetComponent<GameLogic>();
        uiManager = FindObjectOfType<UIManager>();

        SceneManager.sceneLoaded += SceneLoadeded;
    }



    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1) 
    {
        uiManager = FindObjectOfType<UIManager>();

        //if (scene.name == "MapNormal1")

        // Regularly loaded into gameplay from character selection
        if (teams.Count > 0)
        {
            // disable more joining
            GetComponent<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;

            //join prefabs manually for gameplay (spawn the correct prefabs for selected players)


            // TODO: for each player in each team spawn and assign team, controllerIDs, color

            for (int t = 0; t < teams.Count; t++)
            {
                for (int p = 0; p < teams[t].players.Count; p++)
                {
                    // var existingPlayer = teams[0].players[0]; EMPTY, SO REPLACE (BUT WITH CORRECT CONTROLLER)

                    var currPlayer = FindObjectOfType<PlayerSpawner>().spawnPlayer();
                    teams[t].players[p] = currPlayer;
                    FindObjectOfType<PlayerSpawner>().playerJoined(currPlayer.transform);
                    currPlayer.GetComponentInChildren<PlayerVisuals>().initColor(getColorOf(currPlayer));
                }
            }
            

            //newPlayer.GetComponent<PlayerInput>().device
            // MANUALLY JOIN TO MANAGER?
            //FindObjectOfType<PlayerInputManager>().join(newPlayer);


        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        if (gameLogic.gameMode.maxTeams > teams.Count)
        {
            //if in menu scene do new teams
            //else if gameplay: no new teams, instead just spawn prefab for exising players

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Selection")
            {
                // first just add all to a new team
                newTeam(player.gameObject);

                FindObjectOfType<MenuManager>().playerJoined(player.transform);

                //TODO: check which player? write string P1 for example
            }
            /*else if (teams.Count <= 1)
            {
                // in gameplay, but no teams made yet (so just fast testing from 1 scene in editor)

                // for testing add to a new team each new player                                        // FOR SOME REASON still got called even when coming from scene
                newTeam(player.gameObject);

                FindObjectOfType<PlayerSpawner>().playerJoined(player.transform);
                player.GetComponentInChildren<PlayerVisuals>().initColor(getColorOf(player.gameObject));
            }*/
        }
    }

    public void newTeam(GameObject player)
    {
        Team newTeam = new Team();
        newTeam.players.Add(player);
        teams.Add(newTeam);
        playerIDs.Add(player);
        //TODO: random color
    }

    public int getTeamOf(GameObject player) //for now jsut 0 or 1, limited teams
    {
        for(int i = 0; i < teams.Count; i++)
        {
            int index = teams[i].players.IndexOf(player);
            //print(index);

            if (index != -1)
                return i;
        }

        return -1; //error, maybe throw ex
    }

    public void moveTeam(GameObject player)
    {
        int i = getTeamOf(player);

        teams[i].players.Remove(player);
        print(i);
        i++;

        if (i > teams.Count)
            newTeam(player);
        else if (i > gameLogic.gameMode.maxTeams) // WRONG ?!
        {
            i = 0;
            teams[i].players.Add(player);
        }    
    }

    public Color getColorOf(GameObject player)
    {
        int i = getTeamOf(player);
        return teamColors[i];
    }

    public int getPlayerId(GameObject player)
    {
        return playerIDs.IndexOf(player);
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

}
