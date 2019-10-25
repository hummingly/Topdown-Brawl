using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : IDamageable
{
    private PlayerSpawner playerSpawner;

    public override void Awake()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();

        alwaysShowHp = true;
        base.Awake();
    }

    public override void OnDeath()
    {
        //Time.timeScale = 0; //just for testing, TODO: destroy or hide player until respawn

        if(GetComponent<BotTest>())
            GetComponent<BotTest>().stopChasing();

        playerSpawner.playerDied(this);
    }
}
