using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance = null;

    class Team //or struct?
    {
        public List<GameObject> players = new List<GameObject>();
        public int points;
    }

    [SerializeField] private Color[] playerColors;

    private bool[] usedColors;

    private List<Team> teams = new List<Team>();

    public GameMode gameMode = new GameMode();


    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);

        // prepare gamemode (scritpable obj?)

        gameMode.useTeams = true;


        usedColors = new bool[playerColors.Length];
    }

    void Update()
    {
        
    }

    public void increaseScore(GameObject scoredPoint) //has to be called if a bullet made a kill (keep track which player shot bullet)
    {
        //find team that GO is in and add point
        //if bigger than gamemode max then won
    }

    public void addPlayer(GameObject player)
    {
        if(gameMode.useTeams)
        {
            // first just add all to a new team
            Team newTeam = new Team();
            newTeam.players.Add(player);
            teams.Add(newTeam);
            //TODO: random color
        }
    }

    public int getTeamOf(GameObject player) //for now jsut 0 or 1
    {
        return Random.Range(0, 2);
        //teams.FindIndex(player);
    }







    public Color getRandUnusedColor()
    {
        var randInd = new int[usedColors.Length];
        for (int i = 0; i < randInd.Length; i++)
            randInd[i] = i;

        randInd = ExtensionMethods.shuffle(randInd);

        for (int i = 0; i < usedColors.Length; i++)
            if (!usedColors[randInd[i]])
            {
                usedColors[randInd[i]] = true;
                return playerColors[randInd[i]];
            }

        return Color.black;
    }

    public Color getUnusedColor()
    {
        for (int i = 0; i < usedColors.Length; i++)
            if (!usedColors[i])
            {
                usedColors[i] = true;
                return playerColors[i];
            }

        return Color.black;
    }
}
