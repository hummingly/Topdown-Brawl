using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode //rather scriptable object?
{
    //public enum WinConditions { Kills, Objective };
    public int pointsToWin = 8;
    public enum PointReward { OnKill, OnDestroy, OnOccupyTime, OnReturnFlag, OnGoal, None };
    public PointReward pointsFor = PointReward.None; //skirmish or sandbox per default

    //killing 5 players or one object is no different...

    public bool doRespawn;
    public float startTime;


}
