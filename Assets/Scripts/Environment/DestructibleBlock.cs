using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlock : IDamageable
{

    public override void OnDeath()
    {
        Destroy(gameObject);
    }
}
