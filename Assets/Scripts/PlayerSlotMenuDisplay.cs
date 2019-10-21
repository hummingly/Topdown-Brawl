using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotMenuDisplay : MonoBehaviour
{
    public Character chara;

    public Image displaySprite;



    public void setSlot(Character chara, Color col)
    {
        this.chara = chara;

        displaySprite.sprite = chara.sprite;
        displaySprite.color = col;
}

}
