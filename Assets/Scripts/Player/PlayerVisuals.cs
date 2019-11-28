using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private float dashUsedBrigthness = 0.5f;

    private Color mainColor;

    void Start()
    {

    }

    void Update()
    {
        
    }



    public void InitColor(Color col)
    {
        SetColorAll(col);
        mainColor = col;
    }

    public void SetActionOnCooldownCol()
    {
        SetColorAll(ExtensionMethods.turnTeamColorDark(mainColor, dashUsedBrigthness));
    }
    public void SetMainColor()
    {
        SetColorAll(mainColor);
    }

    private void SetColorAll(Color col)
    {
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.color = col;
    }
}
