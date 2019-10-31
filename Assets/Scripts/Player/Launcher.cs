using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public enum Purpose { Basic, Secondary };
    [SerializeField] private Purpose purpose;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed = 40;
    [SerializeField] private float cooldown = 0.125f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float spawnPosFromCenter = 0.75f;

    private float delayTimer;
    private float shootInput;
    // for ztrigger, input value.Get<float> is not 0 but a small number
    private float inputtolerance = 0.8f;

    public void Init (GameObject projectile, Purpose purpose, float speed, float cooldown, int damage)
    {
        this.projectile = projectile;
        this.purpose = purpose;
        this.speed = speed;
        this.cooldown = cooldown;
        this.damage = damage;
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        delayTimer -= Time.deltaTime;

        if (shootInput > inputtolerance && delayTimer <= 0)
        {
            delayTimer = cooldown;
            shoot(playerMovement.getLastRot());
        }
    }

    public void shoot(Vector2 shootDir) // Don't shoot where player really faces, but where he tries to look at with stick
    {
        GameObject p = Instantiate(projectile, (Vector2)transform.position + (shootDir.normalized * spawnPosFromCenter), Quaternion.identity);
        p.GetComponent<Projectile>().setDamage(damage);
        p.GetComponent<Projectile>().setOwner(gameObject);
        p.transform.up = shootDir;
        p.GetComponent<Rigidbody2D>().AddForce(/*transform.up*/ shootDir.normalized * speed, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), p.GetComponent<Collider2D>());
    }


    private void OnRightTrigger(InputValue value) //https://forum.unity.com/threads/new-input-system-how-to-use-the-hold-interaction.605587/
    {
        if (purpose == Purpose.Basic)
        {
            shootInput = value.Get<float>();
            //print(shootInput);
        }
    }

    

    private void OnZRightTrigger(InputValue value) //https://forum.unity.com/threads/new-input-system-how-to-use-the-hold-interaction.605587/
    {
        if (purpose == Purpose.Secondary)
        {
            shootInput = value.Get<float>();
            // IMPLEMENT AIMING WITH shootInput > 0 and < 0.8
            //print(shootInput);
        }
    }
}
