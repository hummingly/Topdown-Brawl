using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scores;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject gameOver;

    void Update()
    {

    }

    public void UpdateScores(int[] teamKillScores)
    {
        for(int i = 0; i < teamKillScores.Length; i++)
        {
            TextMeshProUGUI text = scores[i].GetComponent<TextMeshProUGUI>();
            text.SetText(teamKillScores[i].ToString());
        }
    }

    public void SetGameOverUI(string winningTeam)
    {
        gameOver.SetActive(true);
        TextMeshProUGUI text = gameOver.GetComponentInChildren<TextMeshProUGUI>();
        text.SetText("Team " + winningTeam + " won!");
    }
}