using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Skill : MonoBehaviour
{
    protected PlayerMovement playerMovement;

    protected enum Trigger { Basic, Secondary };
    protected enum TriggerType { Down, Hold, Up };
    //protected enum Type { Press, Release };
    // put speed and projectile in children because melee skill might not have speed or projectile
    //[SerializeField] private GameObject projectile;
    //[SerializeField] private float speed = 40;
    [SerializeField] protected float cooldown;
    [SerializeField] protected int damage;
    [SerializeField] protected float spawnPosFromCenter;
    [SerializeField] protected Trigger trigger;
    [SerializeField] protected TriggerType triggerType = TriggerType.Hold;
    // for ztrigger, input value.Get<float> is not 0 but a small number
    //[SerializeField] protected float inputTolerance = 0.8f;

    protected float delayTimer;
    protected float actionInput;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // may be outsourced to children if very different implementation? see for implementation of Update method:
        // https://forum.unity.com/threads/how-to-call-update-from-a-class-thats-not-inheriting-from-monobehaviour.451954/
        delayTimer -= Time.deltaTime;

        if (actionInput > 0 && delayTimer <= 0)
        {
            DoAttack();
        }   
    }

    public void DoAttack()
    {
        delayTimer = cooldown;

        Attack(playerMovement.GetLastRot());
    }

    protected void OnRightTrigger(InputValue value)
    {
        if (trigger == Trigger.Basic && triggerType == TriggerType.Hold)
            // depending on other (non-shooting) skills put here directly: shootInput = value.Get<float>();
            OnTrigger(value.Get<float>());
    }

    protected void OnRightTriggerDown(InputValue value)
    {
        if (trigger == Trigger.Basic && triggerType == TriggerType.Down)
            OnTriggerDown();
    }

    protected void OnZRightTrigger(InputValue value)
    {
        if (trigger == Trigger.Secondary && triggerType == TriggerType.Hold)
            // depending on other (non-shooting) skills put here directly: shootInput = value.Get<float>();
            OnTrigger(value.Get<float>());
    }

    protected void OnZRightTriggerUp(InputValue value)
    {
        OnTriggerUp(value.Get<float>());
    }

    protected abstract void OnTrigger(float inputValue);
    protected abstract void OnTriggerUp(float inputValue);
    protected abstract void OnTriggerDown();

    protected abstract void Attack(Vector2 shootDir); // Don't shoot where player really faces, but where he tries to look at with stick

    // for bot shooting
    public void SetAttacking(bool b)
    {
        actionInput = b ? 1 : 0;
    }

}
