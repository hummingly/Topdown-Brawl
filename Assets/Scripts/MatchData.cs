using System;
using System.Collections.Generic;
using UnityEngine;

// DataStructure passed between MatchMaking, Ingame and End States/Scenes.
public class MatchData : MonoBehaviour
{
    [Serializable]
    public struct MatchRules
    {
        // TODO: Number of rounds.
        public readonly GameMode.WinCondition WinCondition;
        public readonly int MaxPoints;
        public readonly int MaxTeamSize;
        public readonly int TeamCount;

        // TODO: Base max size on actual team number.
        public MatchRules(GameMode.WinCondition winCondition, int maxPoints, int maxTeamSize, int teamCount)
        {
            WinCondition = winCondition;
            MaxPoints = maxPoints;
            MaxTeamSize = maxTeamSize;
            TeamCount = teamCount;
        }
    }

    [SerializeField] private string map = "";
    [SerializeField] private string mode = "";
    [SerializeField] private WinManager score;
    [SerializeField] private TeamManager teamManager;

    public string Map => map;

    public string Mode => mode;

    public TeamManager TeamManager => teamManager;

    public WinManager Score => score;

    public void Setup(string map, GameMode gameMode, TeamManager teamManager)
    {
        this.map = map;
        this.mode = gameMode.name;
        score = new WinManager(
            new MatchRules(gameMode.winCondition, gameMode.pointsToWin, gameMode.maxTeamSize, teamManager.Count)
        );
        this.teamManager = teamManager;
    }
}