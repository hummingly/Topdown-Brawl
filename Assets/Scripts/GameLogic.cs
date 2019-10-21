using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    class Team //struct
    {
        public List<GameObject> players = new List<GameObject>();
        public int points;
    }

    [SerializeField] private Color[] playerColors;

    private bool[] usedColors;

    private List<Team> teams = new List<Team>();

    public GameMode gameMode = new GameMode();

    private UIManager uiManager;


    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);

        // prepare gamemode (scritpable obj?)
        gameMode.useTeams = true;

        uiManager = GetComponent<UIManager>();

        usedColors = new bool[playerColors.Length];
    }

    void Update()
    {
        
    }

    public void increaseScore(GameObject player) //has to be called if a bullet made a kill (keep track which player shot bullet)
    {
        int team = getTeamOf(player);
        teams[team].points += 1;
        // display new score in UI
        uiManager.updateScore(team);
        //if bigger than gamemode max then won
        if (teams[team].points >= gameMode.pointsToWin)
        {
            uiManager.setGameOver();
        }
    }

    public int getScore(int team)
    {
        return teams[team].points;
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

        //return teams.player.FindIndex(o => o == player);
        //return Random.Range(0, 2);
        foreach (Team team in teams)
        {
            int i = team.players.FindIndex(o => o == player);
            print(i);
        }
        return 0;
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
