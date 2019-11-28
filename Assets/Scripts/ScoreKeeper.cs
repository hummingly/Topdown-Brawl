using System;

public partial class MatchData
{
    [Serializable]
    public class ScoreKeeper
    {
        private MatchRules matchRules;
        private int winningTeam;
        // TODO: Also keep track for invidual players.
        private int[] killTeamScores = new int[2];
        private DestructibleTeamBlock[] defenseTeamBlocks = new DestructibleTeamBlock[2];

        public int[] TeamKills => killTeamScores;
        public GameMode.WinCondition WinCondition => matchRules.WinCondition;
        public int GetWinningTeam() => winningTeam;

        public ScoreKeeper(MatchRules matchRules)
        {
            this.matchRules = matchRules;
            killTeamScores = new int[matchRules.TeamCount];
        }

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

        private int CheckDefenses()
        {
            var livingTeams = 0;
            var lastIndex = 0;
            for (int i = 0; i < defenseTeamBlocks.Length; i++)
            {
                if (defenseTeamBlocks[i].IsDead)
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
            // checks for a team that won
            return Array.FindIndex(killTeamScores, t => t >= matchRules.MaxPoints);
        }

        // Later we could track deaths and assists too.
        public void IncreaseKillScore(int team)
        {
            killTeamScores[team] += 1;
        }

        private void DestroyTeamDefense(int team)
        {
            defenseTeamBlocks[team] = null;
        }

        public void SetDefenseBases(DestructibleTeamBlock[] defenseBases, int team)
        {
            defenseTeamBlocks = new DestructibleTeamBlock[team];
            var len = Math.Min(team, defenseBases.Length);
            for (int i = 0; i < len; i++)
            {
                defenseTeamBlocks[i] = defenseBases[i];
            }
        }

        public int GetTeamKillScore(int team)
        {
            return killTeamScores[team];
        }
    }
}
