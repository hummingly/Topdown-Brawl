using System;
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

        public Team(int teamSize)
        {
            players = new List<GameObject>(teamSize);
            points = 0;
        }
    }

    public List<Team> teams = new List<Team>();
    public List<GameObject> playerIDs = new List<GameObject>();

    [SerializeField] private Color[] teamColors;


    private void Awake()
    {
        teamColors = ExtensionMethods.shuffle(teamColors);

        SceneManager.sceneLoaded += SceneLoadeded;

        GameMode gameMode = GetComponent<GameLogic>().gameMode;
        teams = new List<Team>(gameMode.maxTeams);
        for (int i = 0; i < gameMode.maxTeams; i++)
        {
            teams.Add(new Team(gameMode.maxTeamSize));
        }
    }

    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        // Regularly loaded into gameplay from character selection
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1") //if (teams.Count > 0)
        {
            // disable more joining
            GetComponent<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;

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

                        currPlayer = FindObjectOfType<PlayerSpawner>().spawnBot();
                    }
                    else
                    {
                        // var existingPlayer = teams[0].players[0]; // EMPTY since the player cursor got deleted on scene load, SO REPLACE (BUT WITH CORRECT CONTROLLER?)

                        currPlayer = FindObjectOfType<PlayerSpawner>().spawnPlayer();
                        //newPlayer.GetComponent<PlayerInput>().device
                    }

                    teams[t].players[p] = currPlayer;
                    FindObjectOfType<PlayerSpawner>().playerJoined(currPlayer.transform);
                    currPlayer.GetComponentInChildren<PlayerVisuals>().initColor(getColorOf(currPlayer));
                }
            }
        }
    }


    public void addBot()
    {
        //GameObject bot = Instantiate(cursor, transform.position, Quaternion.identity); //instantiate a new unused cursor
        // not needed since players will change everything for the bot?

        GameObject bot = new GameObject("Empty Bot Cursor");

        if (addToEmptyOrSmallestTeam(bot))
        {
            FindObjectOfType<MenuManager>().playerJoined(bot.transform, true);
            bot.transform.parent = null;
            DontDestroyOnLoad(bot);
        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        //if (!gameLogic.gameMode.maxTeams > teams.Count) return;

        //if in menu scene do new teams
        //else if gameplay: no new teams, instead just spawn prefab for exising players

        if (SceneManager.GetActiveScene().name == "Selection")
        {
            // first just add all to a new team
            if (addToEmptyOrSmallestTeam(player.gameObject))
            {
                FindObjectOfType<MenuManager>().playerJoined(player.transform);
            }
            //TODO: check which player? write string P1 for example
        }

        //else if (teams.Count <= 1)// FOR SOME REASON still got called even when coming from scene
        if (SceneManager.GetActiveScene().name == "gameplayDEV")
        {
            // in gameplay, but no teams made yet (so just fast testing from 1 scene in editor)

            // for testing add to a new team each new player
            if (addToEmptyOrSmallestTeam(player.gameObject))
            {
                FindObjectOfType<PlayerSpawner>().playerJoined(player.transform);
                player.GetComponentInChildren<PlayerVisuals>().initColor(getColorOf(player.gameObject));
            }
        }
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        int team = getTeamOf(player.gameObject);
        if (team > -1)
        {
            teams[team].players.Remove(player.gameObject);
        }
    }

    // Adds player and bots to the team.
    //
    // If a team is full and a player wants to join, a bot is kicked out
    // automatically for this player if possible. Returns true when a bot or a
    // player is added else false.
    public bool addToTeam(GameObject player, int team)
    {
        // Checks whether the team is already full.
        if (teams[team].players.Count >= teams[team].players.Capacity)
        {
            // Players are always added as long there is enough space.
            if (player.GetComponent<BotTest>() == null)
            {
                // Search for bot to replace the player with.
                int slot = teams[team].players.FindIndex(b => b.GetComponent<BotTest>() != null);
                if (slot > -1)
                {
                    playerIDs.Remove(teams[team].players[slot]);
                    teams[team].players[slot] = player;
                    playerIDs.Add(player);
                    return true;
                }
            }
            return false;
        }
        else
        {
            teams[team].players.Add(player);
            playerIDs.Add(player);
            return true;
        }
    }

    public bool addToEmptyOrSmallestTeam(GameObject player)
    {
        if (getEmptyTeam() != -1)
            return addToTeam(player, getEmptyTeam());
        else
            return addToTeam(player, getSmallestTeam());
    }

    public void moveTeam(GameObject player) //can only cycle in one dir through teams
    {
        int currentTeam = getTeamOf(player);

        if (currentTeam == -1)
        {
            addToEmptyOrSmallestTeam(player);
            return;
        }

        int nextTeam = teams.FindIndex(currentTeam + 1 % teams.Count, t => t.players.Count < t.players.Capacity);
        if (nextTeam > -1)
        {
            teams[currentTeam].players.Remove(player);
            teams[nextTeam].players.Add(player);
            return;
        }
        else
        {
            // Look for empty slot in team before the current team.
            int previousTeam = teams.FindIndex(0, currentTeam, t => t.players.Count < t.players.Capacity);
            if (previousTeam > -1)
            {
                teams[currentTeam].players.Remove(player);
                teams[previousTeam].players.Add(player);
            }
        }
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

    // Returns the index of the player's team or -1 if the player has no team.
    public int getTeamOf(GameObject player) //for now jsut 0 or 1, limited teams
    {
        for (int i = 0; i < teams.Count; i++)
        {
            int index = teams[i].players.IndexOf(player);
            if (index != -1)
                return i;
        }

        return -1;
    }


    public Color getColorOf(GameObject player)
    {
        int i = getTeamOf(player);
        return teamColors[i];
    }

    public GameObject getPlayerByID(int i)
    {
        return playerIDs[i];
    }
    public int getPlayerId(GameObject player)
    {
        return playerIDs.IndexOf(player);
    }

    public void increaseScore(GameObject player)
    {
        //find team that GO is in and add point
        int team = getTeamOf(player);

        teams[team].points++;
    }

    public bool someTeamWon(int pointsToWin)
    {
        //if bigger than gamemode max then won
        foreach (Team t in teams)
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
