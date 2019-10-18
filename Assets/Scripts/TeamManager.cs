using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour // Singleton instead of static, so can change variable in inspector
{
    public static TeamManager instance = null;


    [SerializeField] private Color[] playerColors;

    private bool[] usedColors;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        usedColors = new bool[playerColors.Length];
    }


    public Color getRandUnusedColor()
    {
        var randInd = new int[usedColors.Length];
        for (int i = 0; i < randInd.Length; i++)
            randInd[i] = i;

        randInd = ExtensionMethods.shuffle(randInd);

        for (int i = 0; i < usedColors.Length; i++)
            if (!usedColors[randInd[i]])
            {
                usedColors[randInd[i]] = true;
                return playerColors[randInd[i]];
            }

        return Color.black;
    }

    public Color getUnusedColor()
    {
        for(int i = 0; i < usedColors.Length; i++)
            if(!usedColors[i])
            {
                usedColors[i] = true;
                return playerColors[i];
            }

        return Color.black;
    }
}
