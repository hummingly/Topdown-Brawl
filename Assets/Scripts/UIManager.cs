using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scores;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject gameOver;

    private TeamManager teams;

    void Start()
    {
        teams = FindObjectOfType<TeamManager>();

        if (FindObjectOfType<WinManager>().gameMode.winCondition == GameMode.WinCondition.Defense)
            scores[0].transform.parent.gameObject.SetActive(false);
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
        gameplay.SetActive(false);
        gameOver.SetActive(true);

        TextMeshProUGUI text = gameOver.GetComponentInChildren<TextMeshProUGUI>();
        var winningTeam = teams.GetTeamName(FindObjectOfType<WinManager>().GetWinningTeam());
        text.SetText(winningTeam + " WON!");
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