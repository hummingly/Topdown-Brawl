using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlock : IDamageable
{
    SpriteDestruction destruct;

    private void Start()
    {
        destruct = GetComponent<SpriteDestruction>();
        if (destruct && destruct.enabled)
            GetComponent<Collider2D>().enabled = false;
    }

    public override void OnReduceHealth(int amount, Vector3 projectilePos, Vector3 nextProjectilePos)
    {
        if(destruct && destruct.enabled)
        {
            print("actual %: " + (float)healthPoints / maxHealthPoints);
            //print("piece %: " + (float)healthPoints / maxHealthPoints);
            destruct.activateAndExplodePieces(projectilePos, nextProjectilePos, amount, 0.1f, 0.1f);
        }
    }


    public override void OnDeath()
    {
        if (destruct && destruct.enabled)
            destruct.destroy();
        StartCoroutine(destroyAfterFrame());
    }
    private IEnumerator destroyAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
