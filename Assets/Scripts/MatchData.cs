using System;
using UnityEngine;

// DataStructure passed between MatchMaking, Ingame and End States/Scenes.
public partial class MatchData : MonoBehaviour
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
    [SerializeField] private ScoreKeeper score;
    [SerializeField] private TeamManager teamManager;

    public string Map => map;

    public string Mode => mode;

    public TeamManager TeamManager => teamManager;

    public ScoreKeeper Score => score;

    private void Awake() {
        // When map selection scene exists, this will probably not be needed anymore.
        // Due to MatchData being created in Selection, we need to destroy the new
        // one on replay... It's similar to the singleton pattern with the only
        // benefit that we can always reason about its lifetime via the
        // GameStateManager because statics exist for the whole application
        // lifetime unless explicitly destroyed which defeats the points then.
        var others = FindObjectsOfType<MatchData>();
        if (others.Length > 1) {
            // Only on replaying the match, only two instances can exist!
            var matchMaker = FindObjectOfType<MatchMaker>();
            matchMaker.Setup(Array.Find(others, o => o.gameObject != gameObject));
            Destroy(gameObject);
            return;
        }
        teamManager = GetComponent<TeamManager>();
    }

    public void Setup(string map, GameMode gameMode, TeamManager teamManager)
    {
        this.map = map;
        this.mode = gameMode.name;
        score = new ScoreKeeper(
            new MatchRules(gameMode.winCondition, gameMode.pointsToWin, gameMode.maxTeamSize, teamManager.Count)
        );
        this.teamManager = teamManager;
    }
}