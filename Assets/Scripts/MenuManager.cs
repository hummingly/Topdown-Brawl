using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private string scene = "MapNormal1";

    void Start()
    {
        
    }


    void Update()
    {
        //move cursor depending on players, change map, when all ready start in x seconds, etc
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
