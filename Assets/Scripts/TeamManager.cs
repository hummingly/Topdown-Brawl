﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public partial class TeamManager : MonoBehaviour
{
    private MenuManager menu;
    private PlayerSpawner spawner;
    internal List<Team> teams = new List<Team>();
    [SerializeField] private Color[] teamColors;
    [SerializeField] private string[] teamNames;
    public bool debugFastJoin;

    // Actual players
    public List<GameObject> playerNrs = new List<GameObject>();
    public List<InputDevice> playerDevices = new List<InputDevice>();
    public List<Character> playerChars = new List<Character>();

    public int Count => teams.Count;

    private void Awake()
    {
        menu = FindObjectOfType<MenuManager>();
        int seed = UnityEngine.Random.Range(0, 1000);
        teamColors = (Color[])ExtensionMethods.Shuffle(teamColors, seed);
        teamNames = (string[])ExtensionMethods.Shuffle(teamNames, seed);

        var gameMode = menu.SelectedGameMode;
        teams = new List<Team>(gameMode.maxTeams);
        for (int i = 0; i < gameMode.maxTeams; i++)
        {
            teams.Add(new Team(gameMode.maxTeamSize));
        }
    }

    public void InitPlayers()
    {
        Debug.Log("Init Player");
        spawner = FindObjectOfType<PlayerSpawner>();
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
                team.ReplacePlayer(player, currentPlayer);
                spawner.PlayerJoined(currentPlayer.transform);
                currentPlayer.GetComponentInChildren<PlayerVisuals>().InitColor(GetColorOf(currentPlayer));
                FindObjectOfType<EffectManager>().AddGridLigth(0.1f, 3.5f, currentPlayer.GetComponentInChildren<SpriteRenderer>(), currentPlayer.transform);
            }
        }
    }

    public void AddBot(int addBotButtonIndex)
    {
        int team = FindSmallestTeam();
        if (team <= -1)
        {
            return;
        }
        GameObject bot = new GameObject("Empty Bot Cursor");
        if (teams[team].AddPlayer(bot))
        {
            FindObjectOfType<MenuManager>().PlayerJoined(bot.transform, true, addBotButtonIndex);
            bot.transform.parent = null;
            DontDestroyOnLoad(bot);
        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log("Player joined");
        //if in menu scene do new teams
        //else if gameplay: no new teams, instead just spawn prefab for exising players
        if (SceneManager.GetActiveScene().name == "Selection")
        {
            // first just add all to a new team
            if (AddToSmallestTeam(player.gameObject))
            {
                FindObjectOfType<MenuManager>().PlayerJoined(player.transform);
            }

            if (debugFastJoin)
            {
                AddBot(0);
                menu.Play();
            }
            return;
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

        int nextTeam = teams.FindIndex(currentTeam + 1 % teams.Capacity, t => t.Count < t.Capacity);
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
            if (!team.HasPlayer(player))
            {
                List<GameObject> activePlayers = team.FilterPlayers(p => p.activeInHierarchy);
                if (activePlayers.Count <= 0) {
                    break;
                }
                int r = UnityEngine.Random.Range(0, activePlayers.Count);
                return activePlayers[r];
            }
        }
        return null;
    }

    // Returns smallest team or -1 if all teams are full.
    private int FindSmallestTeam()
    {
        // Returns a list of teams which could add a player (empty spot or replace
        // bot). If the list is empty, all teams are already filled with players.
        List<Team> openTeams = teams.FindAll(t => t.Count < t.Capacity || t.ExistsPlayer(p => p.GetComponent<MenuCursor>() == null));
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

    public Color GetColorOf(GameObject player)
    {
        return GetColor(FindPlayerTeam(player));
    }

    public Color GetColor(int team)
    {
        if (team > -1)
        {
            return teamColors[team];
        }
        return new Color(0.0f, 0.0f, 0.0f, 1.0f);
    }

    public string GetTeamName(int team)
    {
        if (team > -1) {
            return teamNames[team];
        }
        return "TEAM NOT FOUND";
    }

    public List<Team> GetTeams()
    {
        return teams;
    }

    public void ColorSpawns()
    {
        for (int t = 0; t < teams.Count; t++)
        {
            var spawnArea = spawner.getSpawnArea(t);
            spawnArea.GetComponent<SpriteRenderer>().color = ExtensionMethods.turnTeamColorDark(GetColor(t), 0.5f);
        }
    }

    public void Wipe()
    {
        teams = new List<Team>(new Team[2]);
        playerNrs.Clear();
        playerDevices.Clear();
        playerChars.Clear();
    }
}
