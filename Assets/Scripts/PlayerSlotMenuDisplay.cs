using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotMenuDisplay : MonoBehaviour
{
    public Role role;

    public Image displaySprite;

    public bool isBot;


    public void setSlot(Role role, Color col, bool bot)
    {
        setChar(role);
        setCol(col);
        isBot = bot;
    }


    public void setChar(Role role)
    {
        this.role = role;

        displaySprite.sprite = this.role.sprite;
    }

    public void setCol(Color col)
    {
        displaySprite.color = col;
    }

}
