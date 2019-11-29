using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : IDamageable
{
    private PlayerSpawner playerSpawner;
    private SoundsPlayer sounds;
    private GameLogic logic;
    private PlayerVisuals visuals;


    public override void Awake()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        logic = FindObjectOfType<GameLogic>();
        visuals = GetComponentInChildren<PlayerVisuals>();
        sounds = GetComponentInChildren<SoundsPlayer>();

        alwaysShowHp = true;
        base.Awake();
    }

    public override void OnDeath()
    {
        //Time.timeScale = 0; //just for testing, TODO: destroy or hide player until respawn

        if (GetComponent<BotTest>())
        {
            GetComponent<BotTest>().StopChasing();
        }

        sounds.Death();
        effects.PlayerDeath(transform.position, GetComponent<PlayerInput>() == null ? null : (Gamepad)GetComponent<PlayerInput>().devices[0]);

        foreach (DefaultShootSkill d in GetComponents<DefaultShootSkill>())
        {
            d.DisableLaser();
        }

        if (damagedLastBy)
        {
            logic.IncreaseScore(damagedLastBy);
        }
        logic.SetDeathEvent(transform.position);

        playerSpawner.PlayerDied(this);
    }

    public override void OnReduceHealth(int amount, Vector3 projectilePos, Vector3 nextProjectilePos)
    {
        // TODO: effects, whatever
        sounds.Damaged();
        effects.GotDamaged(visuals);
    }
}
