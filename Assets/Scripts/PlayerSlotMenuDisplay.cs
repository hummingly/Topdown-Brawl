using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotMenuDisplay : MonoBehaviour
{
    public Character chara;

    public Image displaySprite;

    public bool isBot;


    public void setSlot(Character character, Color col, bool bot)
    {
        setChar(character);
        setCol(col);
        isBot = bot;
    }


    public void setChar(Character character)
    {
        chara = character;

        displaySprite.sprite = chara.sprite;
    }

    public void setCol(Color col)
    {
        displaySprite.color = col;
    }

}
