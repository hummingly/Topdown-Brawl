using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IDamageable : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private RectTransform statsUi;
    [SerializeField] protected int maxHealthPoints = 100;
    [SerializeField] protected float spawnProtectTime = 2f;
    [SerializeField] protected float spawmProtectMaxDist = 3f;


    internal bool alwaysShowHp; //or hide slider until took damage

    private readonly Quaternion guiRotation = Quaternion.identity;

    protected int healthPoints;
    protected GameObject damagedLastBy;
    private bool invincible;
    protected EffectManager effects;
    protected GameObject invincibleGO;
    protected Vector2 lastSpawnPos;
    private bool alive = true;

    public virtual void Awake()
    {
        effects = FindObjectOfType<EffectManager>();
        healthPoints = maxHealthPoints;
        healthSlider.maxValue = maxHealthPoints;
        if (!alwaysShowHp)
            healthSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        healthSlider.value = healthPoints;
        statsUi.rotation = guiRotation;

        if (invincible)
            if (Vector2.Distance(transform.position, lastSpawnPos) > spawmProtectMaxDist)
            {
                effects.stopInvincible(invincibleGO);
                invincible = false;
            }
    }

    internal void IncreaseHealth(int amount, bool revive = false)
    {
        if (revive) alive = true;
        healthPoints = Mathf.Min(maxHealthPoints, healthPoints + amount);
        healthSlider.value = healthPoints;
    }

    internal bool ReduceHealth(int amount, GameObject dmgSource = null, Vector3 projectilePos = new Vector3(), Vector3 nextProjectilePos = new Vector3())
    {
        if (invincible) return false;

        if (dmgSource)
            damagedLastBy = dmgSource;

        if (!healthSlider.gameObject.active)
            healthSlider.gameObject.SetActive(true);

        healthPoints = Mathf.Max(0, healthPoints - amount);
        healthSlider.value = healthPoints;

        if (healthPoints <= 0 && alive)
        {
            alive = false;
            OnDeath();
        }

        OnReduceHealth(amount, projectilePos, nextProjectilePos);

        return healthPoints <= 0;
    }

    internal void SetMaxHealth(int maxHealth)
    {
        maxHealthPoints = Mathf.Max(1, maxHealth);
        healthPoints = maxHealth;
        healthSlider.maxValue = maxHealthPoints;
        healthSlider.value = maxHealthPoints;
    }

    public abstract void OnDeath();
    public abstract void OnReduceHealth(int amount, Vector3 projectilePos = new Vector3(), Vector3 nextProjectilePos = new Vector3());
    
    public int GetHealth()
    {
        return healthPoints;
    }

    public void SetInvincible(Vector2 spawnPos)
    {
        lastSpawnPos = spawnPos;

        invincibleGO = effects.invincible(transform, spawnProtectTime);
        invincible = true;
        StartCoroutine(disableInvinc(spawnProtectTime));
    }

    private IEnumerator disableInvinc(float dur)
    {
        yield return new WaitForSeconds(dur);
        invincible = false;
    }
}
