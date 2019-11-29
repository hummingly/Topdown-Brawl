using System;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    // TODO make private
    [SerializeField] private GameMode gameMode;
    private int winningTeam = -1;
    // TODO: Also keep track for invidual players.
    private int[] killTeamScores = new int[2];
    private DestructibleBlock[] defenseBlocks = new DestructibleBlock[2];

    public int[] TeamKills => killTeamScores;
    public GameMode.WinCondition WinCondition => gameMode.winCondition;

    public bool OnTeamWon()
    {
        switch (WinCondition)
        {
            case GameMode.WinCondition.Kills:
                var k = CheckScores();
                if (k > -1)
                {
                    winningTeam = k;
                    return true;
                }
                return false;
            case GameMode.WinCondition.Defense:
                var d = CheckDefenses();
                if (d > -1)
                {
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
        for (int i = 0; i < defenseBlocks.Length; i++)
        {
            if (defenseBlocks[i].IsDead)
            {
                DestroyTeamDefense(i);
            }
            else
            {
                livingTeams++;
                lastIndex = i;
            }
        }
        if (livingTeams == 1)
        {
            return lastIndex;
        }
        return -1;
    }

    public int CheckScores()
    {
        return Array.FindIndex(killTeamScores, t => t >= gameMode.pointsToWin);
    }

    // Later we could track deaths and assists too.
    public void IncreaseKillScore(int team)
    {
        killTeamScores[team] += 1;
    }

    private void DestroyTeamDefense(int team)
    {
        defenseBlocks[team] = null;
    }

    public void SetGameMode(GameMode mode)
    {
        killTeamScores = new int[mode.maxTeams];
        defenseBlocks = new DestructibleBlock[mode.maxTeams];
    }

    public void InitDefenseBases()
    {
        var parent = GameObject.FindGameObjectWithTag("DefenseBases");
        var blocks = parent.GetComponentsInChildren<DestructibleBlock>();
        var teamManager = FindObjectOfType<TeamManager>();
        var len = Math.Min(teamManager.Count, blocks.Length);
        for (int i = 0; i < len; i++)
        {
            // the order of the destructible team blocks (in the parent) has to be the same as for the spawn areas!
            defenseBlocks[i] = blocks[i];
            var meshes = blocks[i].gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var m in meshes)
            {
                m.material.color = teamManager.GetColor(i);
            }
        }
    }

    public void ResetRound()
    {
        winningTeam = -1;
        Array.Clear(killTeamScores, 0, killTeamScores.Length);
        Array.Clear(defenseBlocks, 0, defenseBlocks.Length);
    }
}
