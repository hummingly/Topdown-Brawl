using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode //rather scriptable object?
{
    //public enum WinConditions { Kills, Objective };
    public int pointsToWin;
    public enum PointReward { OnKill, OnDestroy, OnOccupyTime, OnReturnFlag, OnGoal, None };
    public PointReward pointsFor = PointReward.None; //skirmish or sandbox per default

    //killing 5 players or one object is no different...

    public bool doRespawn;
    public float startTime;
    public int stockCount;

    //public enum TeamMode { FFA, Teams };
    //public TeamMode mode = TeamMode.Teams;
    public bool useTeams;
}
