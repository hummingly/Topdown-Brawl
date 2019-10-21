using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    struct Team //or class?
    {
        public List<GameObject> players;
        public int points;
    }

    private List<Team> teams = new List<Team>();

    public GameMode gameMode = new GameMode();


    void Awake()
    {
        // prepare gamemode (scritpable obj?)

        gameMode.useTeams = true;
    }

    void Update()
    {
        
    }

    public void increaseScore(GameObject scoredPoint) //has to be called if a bullet made a kill (keep track which player shot bullet)
    {
        //find team that GO is in and add point
        //if bigger than gamemode max then won
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
