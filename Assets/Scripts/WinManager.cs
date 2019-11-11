using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TeamManager;

public class WinManager : MonoBehaviour
{
    [SerializeField] public GameMode gameMode;

    void Start()
    {

    }

    public bool TeamWon(List<Team> teams)
    {
        switch (gameMode.winCondition)
        {
            case GameMode.WinCondition.Kills:
                return CheckScores(teams);
            case GameMode.WinCondition.Defense:
                return CheckDefenses();
            default:
                return false;
        }

    }

    private bool CheckDefenses()
    {
        throw new NotImplementedException();
    }

    public bool CheckScores(List<Team> teams)
    {
        return teams.Exists(t => t.Points >= gameMode.pointsToWin);
    }

}
