using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scores;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject gameOver;

    private TeamManager teamManager;

    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
    }

    void Update()
    {

    }

    public void UpdateScores()
    {
        for(int i = 0; i < teamManager.Count; i++)
        {
            TextMeshProUGUI text = scores[i].GetComponent<TextMeshProUGUI>();
            text.SetText(teamManager.GetScore(i).ToString());
        }
    }

    public void SetGameOverUI()
    {
        gameOver.SetActive(true);
        TextMeshProUGUI text = gameOver.GetComponentInChildren<TextMeshProUGUI>();
        var winningTeam = teamManager.GetTeamName(FindObjectOfType<WinManager>().GetWinningTeam());
        text.SetText("Team " + winningTeam + " won!");
    }
}