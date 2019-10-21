using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    class Team //or class?
    {
        public List<GameObject> players = new List<GameObject>();
        public int points;
    }

    private List<Team> teams = new List<Team>();

    public GameMode gameMode = new GameMode();

    private UIManager uiManager;


    void Awake()
    {
        // prepare gamemode (scritpable obj?)
        gameMode.useTeams = true;
        uiManager = GetComponent<UIManager>();
    }

    void Update()
    {
        
    }

    public void increaseScore(GameObject player) //has to be called if a bullet made a kill (keep track which player shot bullet)
    {
        int team = getTeamOf(player);
        teams[team].points += 1;
        // display new score in UI
        uiManager.updateScore(team);
        //if bigger than gamemode max then won
        if (teams[team].points >= gameMode.pointsToWin)
        {
            uiManager.setGameOver();
        }
    }

    public int getScore(int team)
    {
        return teams[team].points;
    }

    public void addPlayer(GameObject player)
    {
        if(gameMode.useTeams)
        {
            // first just add all to a new team
            Team newTeam = new Team();
            newTeam.players.Add(player);
            teams.Add(newTeam);
            //TODO: random color
        }
    }

    public int getTeamOf(GameObject player) //for now jsut 0 or 1
    {
        return Random.Range(0, 2);
        //teams.FindIndex(player);
    }
}
