using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private float dashUsedBrigthness = 0.5f;

    private Color mainColor;
    private bool isBlinking;
    private IEnumerator coroutine;

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
        if(!isBlinking)
            SetColorAll(ExtensionMethods.turnTeamColorDark(mainColor, dashUsedBrigthness));
    }
    public void SetMainColor()
    {
        if (!isBlinking)
            SetColorAll(mainColor);
    }


    public void blinkWhite(Color col, int frames)
    {
        //StopAllCoroutines();
        if (coroutine!=null) StopCoroutine(coroutine);
        StartCoroutine(coroutine = blink(col, frames));
    }



    private IEnumerator blink(Color col, int frames)
    {
        isBlinking = true;
        SetColorAll(col);

        //yield return new WaitForEndOfFrame();
        for (int i = 0; i < frames; i++)
            yield return null;

        SetColorAll(mainColor);
        isBlinking = false;
    }

    private void SetColorAll(Color col)
    {
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.color = col;
    }
}
