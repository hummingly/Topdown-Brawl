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
    //public List<GameObject> playerIDs = new List<GameObject>();
    public List<GameObject> playerNrs = new List<GameObject>(); //not for bots, just players... for keeping track of what name to put for each, etc

    [SerializeField] private Color[] teamColors;
    private PlayerSpawner spawner;


    private void Awake()
    {
        teamColors = ExtensionMethods.shuffle(teamColors);

        for(int i = 0; i < GetComponent<GameLogic>().gameMode.maxTeams; i++)
            teams.Add(new Team());
    }



    public void initPlayers()
    {
        // disable more joining
        GetComponent<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        spawner = FindObjectOfType<PlayerSpawner>();

        //join prefabs manually for gameplay (spawn the correct prefabs for selected players)


        // TODO: for each player in each team spawn and assign team, controllerIDs, color

        for (int t = 0; t < teams.Count; t++)
        {
            for (int p = 0; p < teams[t].players.Count; p++)
            {
                GameObject currPlayer = null;

                // Bot controller is not destroyed on scene load, so now destroy it and spawn a bot object, not a player object
                if (teams[t].players[p] != null)
                {
                    Destroy(teams[t].players[p].gameObject);

                    currPlayer = spawner.createBot();
                }
                else
                {
                    // var existingPlayer = teams[0].players[0]; // EMPTY since the player cursor got deleted on scene load, SO REPLACE (BUT WITH CORRECT CONTROLLER?)

                    currPlayer = spawner.createPlayer();
                    //newPlayer.GetComponent<PlayerInput>().device
                }

                teams[t].players[p] = currPlayer;
                spawner.playerJoined(currPlayer.transform);
                currPlayer.GetComponentInChildren<PlayerVisuals>().initColor(getColorOf(currPlayer));
            }
        }
    }


    public void addBot(int addBotButtonIndex)
    {
        //GameObject bot = Instantiate(cursor, transform.position, Quaternion.identity); //instantiate a new unused cursor
        // not needed since players will change everything for the bot?

        GameObject bot = new GameObject("Empty Bot Cursor");

        addToEmptyOrSmallestTeam(bot);

        FindObjectOfType<MenuManager>().playerJoined(bot.transform, true, addBotButtonIndex);

        bot.transform.parent = null;
        DontDestroyOnLoad(bot);
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        //if (!gameLogic.gameMode.maxTeams > teams.Count) return;

        //if in menu scene do new teams
        //else if gameplay: no new teams, instead just spawn prefab for exising players

        if (SceneManager.GetActiveScene().name == "Selection")
        {
            // first just add all to a new team
            addToEmptyOrSmallestTeam(player.gameObject);

            FindObjectOfType<MenuManager>().playerJoined(player.transform);

            //TODO: check which player? write string P1 for example
        }

        //else if (teams.Count <= 1)// FOR SOME REASON still got called even when coming from scene
        if (SceneManager.GetActiveScene().name == "gameplayDEV")
        {
            // in gameplay, but no teams made yet (so just fast testing from 1 scene in editor)

            // for testing add to a new team each new player
            addToEmptyOrSmallestTeam(player.gameObject);

            FindObjectOfType<PlayerSpawner>().playerJoined(player.transform);
            player.GetComponentInChildren<PlayerVisuals>().initColor(getColorOf(player.gameObject));
        }
    }


    public void addToTeam(GameObject player, int i)
    {
        teams[i].players.Add(player);
        //playerIDs.Add(player);

        // if no bot
        if(player.GetComponent<MenuCursor>())
            playerNrs.Add(player);
    }

    public void addToEmptyOrSmallestTeam(GameObject player)
    {
        if (getEmptyTeam() != -1)
            addToTeam(player, getEmptyTeam());
        else
            addToTeam(player, getSmallestTeam());
    }

    public void moveTeam(GameObject player) //can only cycle ion one dir through teams
    {
        int i = getTeamOf(player);

        teams[i].players.Remove(player);
        //print(i);
        i++;

        if (i >= teams.Count)
            i = 0;

        teams[i].players.Add(player);
    }

    public GameObject getRandomEnemy(GameObject player)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            // Found enemy team
            if(!teams[i].players.Contains(player))
            {
                // get random active player
                var randPlayers = ExtensionMethods.shuffle(teams[i].players);
                foreach(GameObject p in randPlayers)
                {
                    if (p.active)
                        return p;
                }
            }
        }

        return null;
    }

    private int getEmptyTeam()
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].players.Count == 0)
                return i;
        }
        return -1;
    }

    private int getSmallestTeam()
    {
        int smallestTeam = int.MaxValue;
        int smallestTeamPlayers = int.MaxValue;
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].players.Count < smallestTeamPlayers)
            {
                smallestTeam = i;
                smallestTeamPlayers = teams[i].players.Count;
            }
        }
        return smallestTeam;
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

    public void remove(GameObject player)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            // Found correct team
            if (teams[i].players.Contains(player))
            {
                //foreach (GameObject p in teams[i].players)
                //    if (p == player)
                        //playerIDs.Remove(player);
                        teams[i].players.Remove(player);
                        return;
            }
        }
    }


    public Color getColorOf(GameObject player)
    {
        int i = getTeamOf(player);
        return teamColors[i];
    }

    /*public GameObject getPlayerByID(int i)
    {
        return playerIDs[i];
    }
    public int getPlayerId(GameObject player)
    {
        return playerIDs.IndexOf(player);
    }*/

    public void increaseScore(GameObject player)
    {
        //find team that GO is in and add point
        int team = getTeamOf(player);

        teams[team].points++;
    }

    public bool someTeamWon(int pointsToWin)
    {
        //if bigger than gamemode max then won
        foreach(Team t in teams)
        {
            if (t.points >= pointsToWin)
                return true;
        }

        return false;
    }

    public int getScore(int team)
    {
        return teams[team].points;
    }

}
