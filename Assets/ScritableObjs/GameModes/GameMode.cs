using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Mode", menuName = "Game Mode")]
public class GameMode : ScriptableObject
{
    public new string name;
    public string description;


    public int pointsToWin;
    //public bool doRespawn;
    //public float startTime;
    public int stockCount;

    public int maxTeams = 2;
    public int maxTeamSize = 3;

    public enum WinCondition { Kills, Defense };
    public WinCondition winCondition;
    //public enum PointReward { OnKill, OnDestroy, OnOccupyTime, OnReturnFlag, OnGoal, None };
    //public PointReward pointsFor = PointReward.None; //skirmish or sandbox per default

    //killing 5 players or one object is no different...

    //public bool doRespawn;
    //public float startTime;

    //public bool onlyUniqueChars; //onlyAllowEachCharOnce;

    //public enum TeamMode { FFA, Teams };
    //public TeamMode mode = TeamMode.Teams;
    //public bool useTeams;
}