using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scores;
    [SerializeField] private GameObject time;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void increaseScore(int team)
    {
        scores[team].GetComponent<TextMesh>
    }
    
}
