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

    private WinManager winManager;

    void Start()
    {
        winManager = FindObjectOfType<WinManager>();

        if (winManager.WinCondition == GameMode.WinCondition.Defense)
        {
            scores[0].transform.parent.gameObject.SetActive(false);
        }
    }

    void Update()
    {

    }

    public void UpdateScores()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            var text = scores[i].GetComponent<TextMeshProUGUI>();
            text.SetText(winManager.TeamKills[i].ToString());
        }
    }

    public void SetGameOverUI()
    {
        gameplay.SetActive(false);
        gameOver.SetActive(true);
        TextMeshProUGUI text = gameOver.GetComponentInChildren<TextMeshProUGUI>();
        var winningTeam = FindObjectOfType<TeamManager>().GetTeamName(winManager.GetWinningTeam());
        text.SetText(winningTeam + " WON!");
    }

    public void GoToMenu()
    {
        FindObjectOfType<GameStateManager>().RestartMatchMaking();
    }

    // public void Restart()
    // {
    //     FindObjectOfType<GameStateManager>().RestartMatch();
    // }
}