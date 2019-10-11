using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : IDamageable
{
    public override void Awake()
    {
        alwaysShowHp = true;
        base.Awake();
    }

    public override void OnDeath()
    {
        Time.timeScale = 0; //just for testing, TODO: destroy or hide player until respawn
    }
}
