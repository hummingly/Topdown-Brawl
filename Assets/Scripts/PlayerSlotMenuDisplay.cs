using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSlotMenuDisplay : MonoBehaviour
{
    public Character chara;

    public Image displaySprite;
    public Image displaySpriteClass;
    public TextMeshProUGUI playerNameTxt;

    public bool isBot;
    public int botDeleteCounter;

    public GameObject myPlayer; //or bot

    public void SetSlot(Transform player, Character character, Color col, bool bot, int playerNr)
    {
        SetChar(character);
        SetCol(col);
        isBot = bot;
        myPlayer = player.gameObject;

        if (bot)
            playerNameTxt.text = "Bot";
        else
            playerNameTxt.text = "Player " + (playerNr + 1).ToString("0");
    }


    public void SetChar(Character character)
    {
        chara = character;

        displaySprite.sprite = chara.sprite;
        displaySpriteClass.sprite = chara.roleIcon;
    }

    public void SetCol(Color col)
    {
        displaySprite.color = col;
        displaySpriteClass.color = col;
    }

}
