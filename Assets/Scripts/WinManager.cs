using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TeamManager;

public class WinManager : MonoBehaviour
{
    // TODO make private
    [SerializeField] public GameMode gameMode;
    private Team winningTeam;

    void Start()
    {

    }

    public bool OnTeamWon(List<Team> teams)
    {
        switch (gameMode.winCondition)
        {
            case GameMode.WinCondition.Kills:
                return CheckScores(teams);
            case GameMode.WinCondition.Defense:
                return CheckDefenses(teams);
            default:
                return false;
        }

    }

    public Team GetWinningTeam()
    {
        return winningTeam;
    }

    private bool CheckDefenses(List<Team> teams)
    {
        //print("checking defenses:");
        // (!checks for a team that lost)
        // start checking when teams are set
        if (teams != null)
        {
            //bool x = teams.Exists(t => t.GetBase().getHealth() <= 0);
            int aliveTeams = 0;
            // just for quick testing
            foreach (Team t in teams)
            {
                if (t.GetBase().getHealth() <= 0)
                {
                    //print(t.Color);
                    t.SetBase(null);
                } else
                {
                    aliveTeams++;
                }
            }
            //print(x);

            if (aliveTeams == 1)
            {
                int index = teams.FindIndex(t => t.GetBase().getHealth() > 0);
                winningTeam = teams[index];
                return true;
            }
            
            return false;
        }
        //print("returned false without checking");
        return false;
    }

    public bool CheckScores(List<Team> teams)
    {
        // checks for a team that won
        //return teams.Exists(t => t.Points >= gameMode.pointsToWin);
        int index = teams.FindIndex(t => t.Points >= gameMode.pointsToWin);
        if (index != -1)
            winningTeam = teams[index];
        return index != -1;
    }

}
