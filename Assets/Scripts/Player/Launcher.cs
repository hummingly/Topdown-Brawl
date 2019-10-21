using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed = 4;
    [SerializeField] private float cooldown = 0.2f;
    [SerializeField] private float spawnPosFromCenter = 0.5f;

    private float delayTimer;
    private float shootInput;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        delayTimer -= Time.deltaTime;

        if (shootInput > 0 && delayTimer <= 0)
        {
            delayTimer = cooldown;
            shoot(playerMovement.getLastRot());
        }
    }

    public void shoot(Vector2 shootDir) // Don't shoot where player really faces, but where he tries to look at with stick
    {
        GameObject p = Instantiate(projectile, (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter), Quaternion.identity);
        p.GetComponent<Projectile>().setOwner(gameObject);
        p.transform.up = shootDir;
        p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }


    private void OnRightTrigger(InputValue value) //https://forum.unity.com/threads/new-input-system-how-to-use-the-hold-interaction.605587/
    {
        shootInput = value.Get<float>();
        if (shootInput > 0)
            delayTimer = 0;
        //if (val.triggered) shootInput = 1;
        //else shootInput = 0;
    }

    // for bot shooting
    public void setShooting(bool b)
    {
        shootInput = b ? 1 : 0;
    }
}
