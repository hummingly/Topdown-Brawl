using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TeamManager;

public class WinManager : MonoBehaviour
{
    [SerializeField] public GameMode gameMode;
    private TeamManager teamManager;

    void Start()
    {
        teamManager = GetComponent<TeamManager>();
    }

    public bool TeamWon()
    {
        switch (gameMode.winCondition)
        {
            case GameMode.WinCondition.Kills:
                return CheckScores();
            case GameMode.WinCondition.Protecc:
                return CheckProteccs();
            default:
                return false;
        }
        
    }

    private bool CheckProteccs()
    {
        List<Team> teams = teamManager.GetTeams();
        foreach (Team t in teams)
        {
            if (t.protecc.gameObject == null)
                return true;
        }

        return false;
    }

    private bool CheckScores()
    {
        List<Team> teams = teamManager.GetTeams();
        //if bigger than gamemode max then won
        foreach (Team t in teams)
        {
            if (t.points >= gameMode.pointsToWin)
                return true;
        }

        return false;
    }
}
