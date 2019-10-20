using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    struct Team //or class?
    {
        private GameObject[] players;
        private int points;
    }

    private Team[] teams;

    public GameMode gameMode;


    void Start()
    {
        // fill teams
        // prepare gamemode (scritpable obj?)
    }

    void Update()
    {
        
    }

    public void increaseScore(GameObject scoredPoint) //has to be called if a bullet made a kill (keep track which player shot bullet)
    {
        //find team that GO is in and add point
        //if bigger than gamemode max then won
    }
}
