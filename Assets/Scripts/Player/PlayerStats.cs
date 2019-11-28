using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            GetComponent<BotTest>().StopChasing();
        
        effects.playerDeath(transform.position, GetComponent<PlayerInput>() == null ? null : (Gamepad)GetComponent<PlayerInput>().devices[0]);

        foreach(DefaultShootSkill d in GetComponents<DefaultShootSkill>())
            d.disableLaser();

        if (damagedLastBy)
            FindObjectOfType<GameLogic>().IncreaseScore(damagedLastBy);

        playerSpawner.PlayerDied(this);
    }

    public override void OnReduceHealth(int amount, Vector3 projectilePos, Vector3 nextProjectilePos)
    {
        // TODO: effects, whatever

        effects.gotDamaged(transform);
    }
}
