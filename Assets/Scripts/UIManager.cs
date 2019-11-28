using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scores;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject gameOver;

    private TeamManager teams;

    void Start()
    {
        teams = FindObjectOfType<TeamManager>();
    }

    void Update()
    {

    }

    public void UpdateScores()
    {
        for(int i = 0; i < teams.teams.Count; i++)
        {
            TextMeshProUGUI text = scores[i].GetComponent<TextMeshProUGUI>();
            text.SetText(teams.GetScore(i).ToString());
        }
    }

    public void SetGameOverUI()
    {
        gameOver.SetActive(true);
        TextMeshProUGUI text = gameOver.GetComponentInChildren<TextMeshProUGUI>();
        var winningTeam = teams.GetTeamName(FindObjectOfType<WinManager>().GetWinningTeam());
        text.SetText("Team " + winningTeam + " won!");
    }

    public void GoToMenu()
    {
        FindObjectOfType<GameStateManager>().GoToSelection();
    }

    public void Restart()
    {
        FindObjectOfType<GameStateManager>().Restart();
    }
}