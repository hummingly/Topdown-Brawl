using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IDamageable : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private RectTransform statsUi;
    [SerializeField] private int maxHealthPoints = 100;

    internal bool alwaysShowHp; //or hide slider until took damage

    private readonly Quaternion guiRotation = Quaternion.identity;

    private int healthPoints;

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

    internal bool ReduceHealth(int amount)
    {
        healthSlider.gameObject.SetActive(true);

        healthPoints = Mathf.Max(0, healthPoints - amount);
        healthSlider.value = healthPoints;

        if (healthPoints <= 0)
        {
            healthSlider.gameObject.SetActive(false);
            OnDeath();
        }

        return healthPoints <= 0;
    }

    internal void SetMaxHealth(int maxHealth)
    {
        maxHealthPoints = Mathf.Max(1, maxHealth);
        healthPoints = maxHealth;
        healthSlider.maxValue = maxHealthPoints;
        healthSlider.value = maxHealthPoints;
    }


    public void respawned()
    {
        IncreaseHealth(int.MaxValue);

        if (!alwaysShowHp)
            healthSlider.gameObject.SetActive(false);
    }

    public abstract void OnDeath();
}
