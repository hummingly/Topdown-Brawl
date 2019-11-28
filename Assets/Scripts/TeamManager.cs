using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public partial class TeamManager : MonoBehaviour
{
    private List<Team> teams = new List<Team>();
    // Actual players
    public List<GameObject> playerNrs = new List<GameObject>();
    public List<InputDevice> playerDevices = new List<InputDevice>();
    public List<Character> playerChars = new List<Character>();

    public int Count => teams.Count;
    public IEnumerable<Team> Teams => teams;

    // TODO: Currently the game mode can be changed while team selection.
    // Maybe have game mode and map selection first?
    public void Setup(int maxTeamCount, int maxTeamSize, string[] names, Color[] teamColors)
    {
        teams = new List<Team>(maxTeamCount);
        for (int i = 0; i < maxTeamCount; i++)
        {
            teams.Add(new Team(maxTeamSize, names[i], teamColors[i]));
        }
    }

    public void InitPlayers()
    {
        Debug.Log("Init Player");
        PlayerSpawner spawner = FindObjectOfType<PlayerSpawner>();
        var input = FindObjectOfType<PlayerInputManager>();

        int index = 0;

        //join prefabs manually for gameplay (spawn the correct prefabs for selected players)

        // TODO: for each player in each team spawn and assign team, controllerIDs, color
        for (int t = 0; t < teams.Count; t++)
        {
            Team team = teams[t];
            for (int p = 0; p < team.Count; p++)
            {
                GameObject player = team.Get(p);
                GameObject currentPlayer = null;
                // Bot cursor is not destroyed on scene load, so now destroy it and spawn a bot, not a player object
                if (player != null)
                {
                    Destroy(player.gameObject);
                    currentPlayer = spawner.CreateBot();
                }
                else
                {
                    // couldn't get it to just repair the device to the joined PlayerInput, so instead just join manually... (alternativly try to change loop order so devices stay correct?)
                    input.playerPrefab = playerChars[index].prefab;
                    input.JoinPlayer(index, -1, null, playerDevices[index]);
                    foreach (PlayerInput playerInput in FindObjectsOfType<PlayerInput>())
                    {
                        if (playerInput.devices[0] == playerDevices[index])
                        {
                            currentPlayer = playerInput.gameObject;
                        }
                    }
                    index++;
                    /* Each PlayerInput can be assigned one or more devices. 
                     * By default, no two PlayerInput components will be assigned the same devices — 
                     * although this can be forced explicitly by manually assigning devices to a player when calling PlayerInput.
                     * Instantiate or by calling InputUser.PerformPairingWithDevice on the InputUser of a PlayerInput */
                }
                team.Replace(player, currentPlayer);
                spawner.PlayerJoined(currentPlayer.transform);
                currentPlayer.GetComponentInChildren<PlayerVisuals>().InitColor(GetColorOf(currentPlayer));
            }
        }
    }

    public void AddBot()
    {
        int team = FindSmallestTeam();
        if (team <= -1)
        {
            return;
        }
        GameObject bot = new GameObject("Empty Bot Cursor");
        if (teams[team].Add(bot))
        {
            var menuManager = FindObjectOfType<MatchMaker>();
            menuManager.PlayerJoined(bot.transform, true);
            menuManager.ToggleReady(bot.transform.gameObject);
            bot.transform.parent = null;
            DontDestroyOnLoad(bot);
        }
    }

    internal void AddPlayerInput(PlayerInput player)
    {
        if (AddToSmallestTeam(player.gameObject))
        {
            FindObjectOfType<MatchMaker>().PlayerJoined(player.transform);
        }
    }

    // Adds player and bots to the team.
    //
    // If a team is full and a player wants to join, a bot is kicked out
    // automatically for this player if possible. Returns true when a bot or a
    // player is added else false.
    public bool AddToTeam(GameObject player, int team)
    {
        // Bots are just added immediately.
        if (player.GetComponent<MenuCursor>() == null)
        {
            return teams[team].Add(player);
        }

        // When no spot could be found on the team, return false immediately.
        if (!teams[team].Add(player) && !teams[team].ReplaceBot(player))
        {
            return false;
        }

        playerNrs.Add(player);
        playerDevices.Add(player.GetComponent<PlayerInput>().devices[0]);
        return true;
    }

    public bool AddToSmallestTeam(GameObject player)
    {
        int openTeam = FindSmallestTeam();
        if (openTeam > -1)
        {
            return AddToTeam(player, openTeam);
        }
        return false;
    }

    public void MoveTeam(GameObject player) //can only cycle ion one dir through teams
    {
        int currentTeam = FindPlayerTeam(player);
        if (currentTeam == -1)
        {
            AddToSmallestTeam(player);
            return;
        }

        int nextTeam = teams.FindIndex(currentTeam + 1 % teams.Capacity, t => t.Count < t.Capacity);
        if (nextTeam > -1)
        {
            teams[currentTeam].Remove(player);
            teams[nextTeam].Add(player);
            return;
        }
        // Look for empty slot in team before the current team.
        int previousTeam = teams.FindIndex(0, currentTeam, t => t.Count < t.Capacity);
        if (previousTeam > -1)
        {
            teams[currentTeam].Remove(player);
            teams[previousTeam].Add(player);
        }
    }

    public GameObject GetRandomEnemy(GameObject player)
    {
        foreach (Team team in teams)
        {
            if (!team.Has(player))
            {
                List<GameObject> activePlayers = team.FilterPlayers(p => p.activeInHierarchy);
                if (activePlayers.Count == 0) {
                    break;
                }
                int r = UnityEngine.Random.Range(0, activePlayers.Count);
                return activePlayers[r];
            }
        }
        return null;
    }

    public int GetTotalPlayers()
    {
        int count = 0;
        foreach (var team in teams)
        {
            count += team.Count;
        }
        return count;
    }

    // Returns smallest team or -1 if all teams are full.
    private int FindSmallestTeam()
    {
        // Returns a list of teams which could add a player (empty spot or replace
        // bot). If the list is empty, all teams are already filled with players.
        List<Team> openTeams = teams.FindAll(t => t.Count < t.Capacity || t.Exists(p => p.GetComponent<MenuCursor>() == null));
        if (openTeams.Count == 0)
        {
            return -1;
        }

        int smallestTeam = 0;
        int smallestTeamPlayers = openTeams[0].Count;
        for (int i = 0; i < openTeams.Count; i++)
        {
            if (openTeams[i].Count < smallestTeamPlayers)
            {
                smallestTeam = i;
                smallestTeamPlayers = openTeams[i].Count;
            }
        }
        return smallestTeam;
    }

    // Returns the index of the player's team or -1 if the player has no team.
    public int FindPlayerTeam(GameObject player)
    {
        return teams.FindIndex(t => t.Has(player));
    }

    public void Remove(GameObject player)
    {
        foreach (Team team in teams)
        {
            if (team.Remove(player))
            {
                break;
            }
        }
    }

    public Color GetColorOf(GameObject player)
    {
        return teams[FindPlayerTeam(player)].Color;
    }

    public string GetTeamName(int team)
    {
        return teams[team].Name;
    }
}
