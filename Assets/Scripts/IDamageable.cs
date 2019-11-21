using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IDamageable : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private RectTransform statsUi;
    [SerializeField] protected int maxHealthPoints = 100;

    internal bool alwaysShowHp; //or hide slider until took damage

    private readonly Quaternion guiRotation = Quaternion.identity;

    protected int healthPoints;

    public virtual void Awake()
    {
        healthPoints = maxHealthPoints;
        healthSlider.maxValue = maxHealthPoints;
        if (!alwaysShowHp)
            healthSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        healthSlider.value = healthPoints;
        statsUi.rotation = guiRotation;
    }

    internal void IncreaseHealth(int amount)
    {
        healthPoints = Mathf.Min(maxHealthPoints, healthPoints + amount);
        healthSlider.value = healthPoints;
    }

    internal bool ReduceHealth(int amount, Vector3 projectilePos = new Vector3(), Vector3 nextProjectilePos = new Vector3())
    {
        if (!healthSlider.gameObject.active)
            healthSlider.gameObject.SetActive(true);

        healthPoints = Mathf.Max(0, healthPoints - amount);
        healthSlider.value = healthPoints;

        if (healthPoints <= 0)
            OnDeath();

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
}
