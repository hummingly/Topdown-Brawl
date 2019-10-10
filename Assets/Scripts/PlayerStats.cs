using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private RectTransform guiTransform;
    private readonly Quaternion guiRotation = Quaternion.identity;

    // Health Points
    public int healthPoints;
    public int maxHealthPoints = 100;
    [SerializeField] private Slider healthpointsUi;

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = maxHealthPoints;
        healthpointsUi.maxValue = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        healthpointsUi.value = healthPoints;
        guiTransform.rotation = guiRotation;
    }

    void IncreaseHealth(int amount)
    {
        healthPoints = Math.Min(maxHealthPoints, healthPoints + amount);
        healthpointsUi.value = healthPoints;
    }

    void ReduceHealth(int amount)
    {
        healthPoints = Math.Max(0, healthPoints - amount);
        healthpointsUi.value = healthPoints;
    }

    void SetMaxHealth(int maxHealth)
    {
        maxHealth = Math.Max(1, maxHealth);
        healthpointsUi.maxValue = maxHealthPoints;
    }
}
