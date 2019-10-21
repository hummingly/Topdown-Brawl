using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scores;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject gameOver;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void updateScore(int team)
    {
        GameLogic gameLogic = GetComponent<GameLogic>();
        TextMeshProUGUI text = scores[team].GetComponent<TextMeshProUGUI>();
        text.SetText(gameLogic.getScore(team).ToString());
    }

    public void setGameOver()
    {
        gameOver.SetActive(true);
    }
    
}
