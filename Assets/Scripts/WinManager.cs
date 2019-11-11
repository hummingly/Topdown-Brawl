﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TeamManager;

public class WinManager : MonoBehaviour
{
    // TODO make private
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
                return CheckDefenses(teams);
            default:
                return false;
        }

    }

    private bool CheckDefenses(List<Team> teams)
    {
        //print("checking defenses:");
        // (!checks for a team that lost)
        // start checking when teams are set
        if (teams != null)
        {
            bool x = teams.Exists(t => t.getBase().getHealth() <= 0);
            //print(x);
            return x;
        }
        //print("returned false without checking");
        return false;
    }

    public bool CheckScores(List<Team> teams)
    {
        // checks for a team that won
        return teams.Exists(t => t.Points >= gameMode.pointsToWin);
    }

}