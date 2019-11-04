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
    }

}