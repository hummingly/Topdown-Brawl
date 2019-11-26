using System;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;
    private int winningTeam;
    // At the moment we will only keep the score per team.
    private int[] killTeamScores;
    private DestructibleTeamBlock[] defenseTeamBlocks;

    public int MaxTeamSize => gameMode.maxTeamSize;
    public int MaxTeamCount => gameMode.maxTeams;
    public int[] TeamKills => killTeamScores;
    public GameMode.WinCondition WinCondition => gameMode.winCondition;

    public void SetGameMode(GameMode mode, int teams) {
        gameMode = mode;
        killTeamScores = new int[teams];
    }

    public bool OnTeamWon()
    {
        switch (gameMode.winCondition)
        {
            case GameMode.WinCondition.Kills:
                var k = CheckScores();
                if (k > -1) {
                    winningTeam = k;
                    return true;
                }
                return false;
            case GameMode.WinCondition.Defense:
                var d = CheckDefenses();
                if (d > -1) {
                    winningTeam = d;
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    public int GetWinningTeam()
    {
        return winningTeam;
    }

    private int CheckDefenses()
    {
        var livingTeams = 0;
        var lastIndex = 0;
        for (int i = 0; i < defenseTeamBlocks.Length; i++)
        {
            if (defenseTeamBlocks[i].IsDeath) {
                defenseTeamBlocks[i] = null;
            } else {
                livingTeams++;
                lastIndex = i;
            }
        }
        if (livingTeams == 1) {
            return lastIndex;
        }
        return -1;
    }

    public int CheckScores()
    {
        // checks for a team that won
        return Array.FindIndex(killTeamScores, t => t >= gameMode.pointsToWin);
    }

    // Later we could track deaths and assists too.
    public void IncreaseKillScore(int team) {
        killTeamScores[team] += 1;
    }

    private void DestroyTeamDefense(int team) {
        defenseTeamBlocks[team] = null;
    }

    public void SetDefenseBases(DestructibleTeamBlock[] defenseBases, int team) {
        defenseTeamBlocks = new DestructibleTeamBlock[team];
        var len = Math.Min(team, defenseBases.Length);
        for (int i = 0; i < len; i++)
        {
            defenseTeamBlocks[i] = defenseBases[i];
        }
    }

    public int GetTeamKillScore(int team) {
        return killTeamScores[team];
    }
}
