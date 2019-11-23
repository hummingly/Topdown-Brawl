using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Skill : MonoBehaviour
{
    protected PlayerMovement playerMovement;
    protected PlayerVisuals visuals;
    protected EffectManager effects;

    protected enum Trigger { Basic, Secondary, Left };
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
    [SerializeField] protected bool showIndicationOnHold;
    [SerializeField] protected bool greyPlayerWhileCD;
    // for ztrigger, input value.Get<float> is not 0 but a small number
    //[SerializeField] protected float inputTolerance = 0.8f;

    protected float delayTimer;
    protected float actionInput;
    
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        visuals = GetComponentInChildren<PlayerVisuals>();
        effects = FindObjectOfType<EffectManager>();
    }

    void Update()
    {
        // may be outsourced to children if very different implementation? see for implementation of Update method:
        // https://forum.unity.com/threads/how-to-call-update-from-a-class-thats-not-inheriting-from-monobehaviour.451954/
        delayTimer -= Time.deltaTime;

        if(delayTimer <= 0)
        {
            if (actionInput > 0)
                DoAction();

            if (greyPlayerWhileCD && visuals)
                visuals.SetMainColor();
        }    
    }

    public void DoAction()
    {
        delayTimer = cooldown;

        if(greyPlayerWhileCD) visuals.SetActionOnCooldownCol();

        Action(playerMovement.GetLastRot());
    }


    // TODO: these triggers and hold/down events need refactoring?
    protected void OnRightTrigger(InputValue value)
    {
        if (trigger == Trigger.Basic && triggerType == TriggerType.Hold)
            OnTrigger(value.Get<float>());
    }

    protected void OnZRightTrigger(InputValue value)
    {
        if (trigger == Trigger.Secondary && triggerType == TriggerType.Hold)
            OnTrigger(value.Get<float>());

        if (trigger == Trigger.Secondary && showIndicationOnHold)
            OnTrigger(value.Get<float>());
    }

    protected void OnZRightTriggerUp(InputValue value)
    {
        if (trigger == Trigger.Secondary && triggerType == TriggerType.Up)
            if (delayTimer <= 0) DoAction(); //actionInput = 1;//OnTriggerUp(value.Get<float>());
    }

    protected void OnRightTriggerDown(InputValue value)
    {
        if (trigger == Trigger.Basic && triggerType == TriggerType.Down)
            if (delayTimer <= 0) DoAction(); //actionInput = 1;//OnTriggerDown();
    }

    private void OnLeftTrigger()
    {
        if (trigger == Trigger.Left)// && triggerType == TriggerType.Down)
            if (delayTimer <= 0) DoAction();// actionInput = 1;//OnTriggerDown();
    }

    protected abstract void OnTrigger(float inputValue);
    //protected abstract void OnTriggerUp(float inputValue);
    //protected abstract void OnTriggerDown();


    protected abstract void Action(Vector2 shootDir); // Don't shoot where player really faces, but where he tries to look at with stick

    // for bot shooting
    public void SetAttacking(bool b)
    {
        actionInput = b ? 1 : 0;
    }

}
