using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Skill : MonoBehaviour
{
    protected PlayerMovement playerMovement;

    protected enum Trigger { Basic, Secondary };
    // put speed and projectile in children because melee skill might not have speed or projectile
    //[SerializeField] private GameObject projectile;
    //[SerializeField] private float speed = 40;
    [SerializeField] protected float cooldown;
    [SerializeField] protected int damage;
    [SerializeField] protected float spawnPosFromCenter;
    [SerializeField] protected Trigger trigger;
    // for ztrigger, input value.Get<float> is not 0 but a small number
    [SerializeField] protected float inputTolerance = 0.8f;

    private float delayTimer;
    protected float shootInput;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // may be outsourced to children if very different implementation? see for implementation of Update method:
        // https://forum.unity.com/threads/how-to-call-update-from-a-class-thats-not-inheriting-from-monobehaviour.451954/
        delayTimer -= Time.deltaTime;

        if (shootInput > inputTolerance && delayTimer <= 0)
        {
            delayTimer = cooldown;
            Attack(playerMovement.getLastRot());
        }
    }
    
    protected void OnRightTrigger(InputValue value)
    {
        if (trigger == Trigger.Basic)
            // depending on other (non-shooting) skills put here directly: shootInput = value.Get<float>();
            OnTrigger(value.Get<float>());
    }

    protected void OnZRightTrigger(InputValue value)
    {
        if (trigger == Trigger.Secondary)
            // depending on other (non-shooting) skills put here directly: shootInput = value.Get<float>();
            OnTrigger(value.Get<float>());
    }

    protected abstract void OnTrigger(float inputValue);

    protected abstract void Attack(Vector2 shootDir); // Don't shoot where player really faces, but where he tries to look at with stick

    // for bot shooting
    public void SetAttacking(bool b)
    {
        shootInput = b ? 1 : 0;
    }

}
