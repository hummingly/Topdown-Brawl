using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : IDamageable
{
    private PlayerSpawner playerSpawner;
    private EffectManager effects;

    public override void Awake()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        effects = FindObjectOfType<EffectManager>();

        alwaysShowHp = true;
        base.Awake();
    }

    public override void OnDeath()
    {
        //Time.timeScale = 0; //just for testing, TODO: destroy or hide player until respawn

        if(GetComponent<BotTest>())
            GetComponent<BotTest>().StopChasing();
        
        effects.playerDeathExplosion(transform.position, GetComponent<PlayerInput>() == null ? null : (Gamepad)GetComponent<PlayerInput>().devices[0]);
        effects.AddShake(2f);
        effects.Stop(0.05f);


        if (damagedLastBy)
            FindObjectOfType<GameLogic>().IncreaseScore(damagedLastBy);

        playerSpawner.PlayerDied(this);
    }

    public override void OnReduceHealth(int amount, Vector3 projectilePos, Vector3 nextProjectilePos)
    {
        // TODO: effects, whatever

        //effects.AddShake(0.25f);
    }
}
