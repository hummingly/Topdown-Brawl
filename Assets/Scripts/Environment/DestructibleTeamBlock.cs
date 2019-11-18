using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTeamBlock : IDamageable
{
    public override void OnDeath()
    {
        Destroy(gameObject);
    }
}
