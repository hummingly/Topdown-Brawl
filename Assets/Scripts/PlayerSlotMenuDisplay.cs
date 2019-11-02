using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotMenuDisplay : MonoBehaviour
{
    public Character chara;

    public Image displaySprite;

    public bool isBot;
    public int botDeleteCounter;

    public GameObject myPlayer; //or bot

    public void setSlot(Transform player, Character character, Color col, bool bot)
    {
        setChar(character);
        setCol(col);
        isBot = bot;
        myPlayer = player.gameObject;
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
