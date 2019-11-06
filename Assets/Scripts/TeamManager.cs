using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{
    [Serializable]
    public class Team : IEnumerable
    {
        private List<GameObject> players;
        public readonly int Capacity;
        public int Points { get; set; }

        public Team(int teamSize)
        {
            Capacity = teamSize;
            players = new List<GameObject>(Capacity);
            Points = 0;
        }

        public bool AddPlayer(GameObject player)
        {
            if (players.Count < Capacity && player != null)
            {
                players.Add(player);
                return true;
            }
            return false;
        }

        public bool RemovePlayer(GameObject player)
        {
            return players.Remove(player);
        }

        public bool ReplacePlayer(GameObject oldPlayer, GameObject newPlayer)
        {
            if (newPlayer == null)
            {
                throw new Exception("An uninitialized player cannot be added to a team.");
            }

            int index = players.IndexOf(oldPlayer);
            if (index > -1)
            {
                players.Insert(index, newPlayer);
                return true;
            }
            return false;
        }

        // A player can be only added to a full team if there is replaceable bot.
        public bool ReplaceBot(GameObject player)
        {
            int slot = players.FindIndex(b => b.GetComponent<MenuCursor>() == null);
            if (slot > -1)
            {
                players[slot] = player;
                return true;
            }
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            return players.GetEnumerator();
        }

        public List<GameObject> FilterPlayers(Predicate<GameObject> predicate)
        {
            return players.FindAll(predicate);
        }

        public bool HasPlayer(GameObject player)
        {
            return players.Contains(player);
        }

        public bool ExistsPlayer(Predicate<GameObject> predicate)
        {
            return players.Exists(predicate);
        }

        public int Count => players.Count;

        public bool IsEmpty => players.Count == 0;

        public bool IsFull => players.Count == Capacity;
    }

    private MenuManager menu;
    private PlayerSpawner spawner;
    public List<Team> teams = new List<Team>();
    [SerializeField] private Color[] teamColors;

    // Actual players
    public List<GameObject> playerNrs = new List<GameObject>();
    public List<InputDevice> playerDevices = new List<InputDevice>();
    public List<Character> playerChars = new List<Character>();

    private void Awake()
    {
        menu = FindObjectOfType<MenuManager>();

        teamColors = ExtensionMethods.Shuffle(teamColors);

        GameMode gameMode = GetComponent<GameLogic>().gameMode;
        teams = new List<Team>(gameMode.maxTeams);
        for (int i = 0; i < gameMode.maxTeams; i++)
        {
            teams.Add(new Team(gameMode.maxTeamSize));
        }
    }


    public void SaveCharacters()
    {
        foreach (GameObject p in playerNrs)
        {
            var cha = menu.GetCharacterOfPlayer(p);

            playerChars.Add(cha);
        }
    }

    public void InitPlayers()
    {
        spawner = FindObjectOfType<PlayerSpawner>();
        var input = FindObjectOfType<PlayerInputManager>();

        int index = 0;

        //join prefabs manually for gameplay (spawn the correct prefabs for selected players)

        // TODO: for each player in each team spawn and assign team, controllerIDs, color
        foreach (Team team in teams)
        {
            foreach (GameObject player in team)
            {
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
                team.ReplacePlayer(player, currentPlayer);
                spawner.PlayerJoined(currentPlayer.transform);
                currentPlayer.GetComponentInChildren<PlayerVisuals>().InitColor(GetColorOf(currentPlayer));
            }
        }
    }


    public void AddBot(int addBotButtonIndex)
    {
        GameObject bot = new GameObject("Empty Bot Cursor");

        if (AddToSmallestTeam(bot))
        {
            FindObjectOfType<MenuManager>().PlayerJoined(bot.transform, true);
            bot.transform.parent = null;
            DontDestroyOnLoad(bot);
        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        //if in menu scene do new teams
        //else if gameplay: no new teams, instead just spawn prefab for exising players

        if (SceneManager.GetActiveScene().name == "Selection")
        {
            // first just add all to a new team
            if (AddToSmallestTeam(player.gameObject))
            {
                FindObjectOfType<MenuManager>().PlayerJoined(player.transform);
            }
            //TODO: check which player? write string P1 for example
        }

        //else if (teams.Count <= 1)// FOR SOME REASON still got called even when coming from scene
        if (SceneManager.GetActiveScene().name == "gameplayDEV")
        {
            // in gameplay, but no teams made yet (so just fast testing from 1 scene in editor)

            // for testing add to a new team each new player
            if (AddToSmallestTeam(player.gameObject))
            {
                FindObjectOfType<PlayerSpawner>().PlayerJoined(player.transform);
                player.GetComponentInChildren<PlayerVisuals>().InitColor(GetColorOf(player.gameObject));
            }
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
            return teams[team].AddPlayer(player);
        }

        // When no spot could be found on the team, return false immediately.
        if (!teams[team].AddPlayer(player) && !teams[team].ReplaceBot(player))
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

        int nextTeam = teams.FindIndex(currentTeam + 1 % teams.Count, t => t.Count < t.Capacity);
        if (nextTeam > -1)
        {
            teams[currentTeam].RemovePlayer(player);
            teams[nextTeam].AddPlayer(player);
            return;
        }
        // Look for empty slot in team before the current team.
        int previousTeam = teams.FindIndex(0, currentTeam, t => t.Count < t.Capacity);
        if (previousTeam > -1)
        {
            teams[currentTeam].RemovePlayer(player);
            teams[previousTeam].AddPlayer(player);
        }
    }

    public GameObject GetRandomEnemy(GameObject player)
    {
        foreach (Team team in teams)
        {
            if (team.HasPlayer(player))
            {
                List<GameObject> activePlayers = team.FilterPlayers(p => p.active);
                int r = UnityEngine.Random.Range(0, activePlayers.Count);
                return activePlayers[r];
            }
        }
        return null;
    }

    // Returns index of empty team, else -1.
    private int FindEmptyTeam()
    {
        return teams.FindIndex(t => t.IsEmpty);
    }

    // Returns a list of teams which could add a player (empty spot or replace
    // bot). If the list is empty, all teams are already filled with players.
    private List<Team> FindOpenTeams()
    {
        return teams.FindAll(t => t.Count < t.Capacity || t.ExistsPlayer(p => p.GetComponent<MenuCursor>() == null));
    }

    // Returns smallest team or -1 if all teams are full.
    private int FindSmallestTeam()
    {
        List<Team> openTeams = FindOpenTeams();
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
        return teams.FindIndex(t => t.HasPlayer(player));
    }

    public void Remove(GameObject player)
    {
        foreach (Team team in teams)
        {
            if (team.RemovePlayer(player))
            {
                break;
            }
        }
    }

    // If the player has no team, it will return a transparent black.
    public Color GetColorOf(GameObject player)
    {
        int i = FindPlayerTeam(player);
        if (i > -1)
        {
            return teamColors[i];
        }
        return new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public void IncreaseScore(GameObject player)
    {
        int team = FindPlayerTeam(player);
        if (team <= -1)
        {
            throw new Exception("Score can be only increased in a match with a player on one team!");
        }
        teams[team].Points++;
    }

    public bool SomeTeamWon(int pointsToWin)
    {
        return teams.Exists(t => t.Points >= pointsToWin);
    }

    public int GetScore(int team)
    {
        return teams[team].Points;
    }
}
