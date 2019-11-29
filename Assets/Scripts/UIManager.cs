using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scores;
    [SerializeField] private GameObject[] rounds;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject roundOverview;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pointPrefab;

    private WinManager winManager;

    private void Start()
    {
        winManager = FindObjectOfType<WinManager>();

        if (winManager.WinCondition == GameMode.WinCondition.Defense)
        {
            scores[0].transform.parent.gameObject.SetActive(false);
        }
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

    public void ShowRoundMatchUi()
    {
        gameOver.SetActive(false);
        roundOverview.SetActive(true);
        var teamManager = FindObjectOfType<TeamManager>();
        for (int i = 0; i < rounds.Length; i++)
        {
            var text = rounds[i].GetComponentInChildren<TextMeshProUGUI>();
            text.SetText(teamManager.GetTeamName(i));
            var color = teamManager.GetColor(i);
            var points = rounds[i].transform.GetChild(1);
            var roundWins = winManager.TeamPoints[i];
            for (int j = 0; j < roundWins; j++)
            {
                var point = Instantiate(pointPrefab, transform.position, Quaternion.identity).transform;
                point.SetParent(points.transform);
                point.GetComponent<Image>().color = color;
                point.transform.localScale = Vector3.one;
            }
        }
    }
}