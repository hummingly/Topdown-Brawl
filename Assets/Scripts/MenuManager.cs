using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private string scene = "MapNormal1";

    private TeamManager teams;

    [SerializeField] private List<Character> availableChars = new List<Character>();
    
    [SerializeField] private GameObject inputPrompt;
    [SerializeField] private Transform charSlotParent;
    [SerializeField] private Transform cursorParent;
    [SerializeField] private GameObject playerSlotPrefab;

    void Awake()
    {
        teams = FindObjectOfType<TeamManager>();
    }


    void Update()
    {
        // CHECK IF ALL PLAYERS READY
    }



    public void toggleCharacter(GameObject player, int dir)
    {
        var slot = charSlotParent.GetChild(teams.getPlayerId(player)).GetComponent<PlayerSlotMenuDisplay>();
        var lastIndex = availableChars.IndexOf(slot.chara);
        lastIndex += dir;
        if (lastIndex < 0) lastIndex = availableChars.Count-1;
        if (lastIndex >= availableChars.Count) lastIndex = 0;
        slot.setSlot(availableChars[lastIndex], teams.getColorOf(player));

        //TODO: actually spawn different character based on selection
    }


    public void togglePlayerTeam(GameObject player)
    {
        teams.moveTeam(player);
        toggleCharacter(player, 0);
    }

    public void toggleMap()
    {
        // TODO
    }

    public void playerJoined(Transform player)
    {
        inputPrompt.SetActive(false);

        player.parent = cursorParent;
        player.localPosition = Vector3.zero;

        var slot = Instantiate(playerSlotPrefab, transform.position, Quaternion.identity).transform;
        slot.parent = charSlotParent;
        slot.GetComponent<PlayerSlotMenuDisplay>().setSlot(availableChars[0], teams.getColorOf(player.gameObject));
    }

    public void Play()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(scene);
    }

    public void SelectMap(string selection)
    {
        scene = selection;
    }

}
