using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlock : IDamageable //TODO: add option to own a block by a team
{
    SpriteDestruction destruct;

    private void Start()
    {
        destruct = GetComponent<SpriteDestruction>();
        if (destruct && destruct.enabled)
            GetComponent<Collider2D>().enabled = false;
    }

    public override void OnReduceHealth(int damage, Vector3 projectilePos, Vector3 nextProjectilePos)
    {
        if(destruct && destruct.enabled)
        {
            //print("actual %: " + (float)healthPoints / maxHealthPoints);
            //print("piece %: " + (float)healthPoints / maxHealthPoints);
            float radius = ExtensionMethods.Remap(damage, 10, 50, 0.3f, 0.5f);
            destruct.activateAndExplodePieces(projectilePos, nextProjectilePos, damage, radius, 0.1f);
        }
    }

    public override void OnDeath()
    {
        if (destruct && destruct.enabled)
            destruct.destroy();

        FindObjectOfType<GameLogic>().SetDeathEvent(transform.position);

        StartCoroutine(DestroyAfterFrame());
    }

    private IEnumerator DestroyAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
