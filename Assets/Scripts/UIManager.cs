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

    public void updateScore(int team)
    {
        TextMeshProUGUI text = scores[team].GetComponent<TextMeshProUGUI>();
        text.SetText(teams.getScore(team).ToString());
    }

    public void setGameOverUI()
    {
        gameOver.SetActive(true);
    }

}