using System;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    // TODO make private
    [SerializeField] private GameMode gameMode;
    [SerializeField] private int winningTeam = -1;
    [SerializeField] private int[] roundWinner = new int[2];
    // TODO: Also keep track for invidual players.
    private int[] killTeamScores = new int[2];
    private DestructibleBlock[] defenseBlocks = new DestructibleBlock[2];

    public int[] TeamKills => killTeamScores;
    public GameMode.WinCondition WinCondition => gameMode.winCondition;
    private int MinRounds => gameMode.rounds / 2 + 1;
    private bool roundRunning = true;

    public bool OnTeamWon()
    {
        switch (WinCondition)
        {
            case GameMode.WinCondition.Kills:
                var k = CheckScores();
                if (!roundRunning) {
                    return true;
                }
                if (k > -1)
                {
                    roundRunning = false;
                    Debug.Log("Score");
                    roundWinner[k] += 1;
                    return OnRoundWin();
                }
                return false;
            case GameMode.WinCondition.Defense:
                var d = CheckDefenses();
                if (d > -1)
                {
                    roundRunning = false;
                    roundWinner[d] += 1;
                    return OnRoundWin();
                }
                return false;
            default:
                return false;
        }
    }

    // Check if the match has finished yet.
    private bool OnRoundWin() {
        Debug.Log(MinRounds);
        var team = Array.FindIndex(roundWinner, r => r >= MinRounds);
        if (team > -1) {
            winningTeam = team;
            return true;
        }
        return !roundRunning;
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
        gameMode = mode;
        killTeamScores = new int[mode.maxTeams];
        defenseBlocks = new DestructibleBlock[mode.maxTeams];
        roundWinner = new int[mode.maxTeams];
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

    public void Reset() {
        ResetRound();
        Array.Clear(roundWinner, 0, roundWinner.Length);
    }

    public void ResetRound()
    {
        roundRunning = true;
        winningTeam = -1;
        Array.Clear(killTeamScores, 0, killTeamScores.Length);
        Array.Clear(defenseBlocks, 0, defenseBlocks.Length);
    }
}
