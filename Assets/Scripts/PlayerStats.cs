using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private RectTransform statsUi;
    [SerializeField] private int maxHealthPoints = 100;

    private readonly Quaternion guiRotation = Quaternion.identity;

    private int healthPoints;


    void Start()
    {
        healthPoints = maxHealthPoints;
        healthSlider.maxValue = maxHealthPoints;
    }

    void Update()
    {
        healthSlider.value = healthPoints;
        statsUi.rotation = guiRotation;
    }



    void IncreaseHealth(int amount)
    {
        healthPoints = Math.Min(maxHealthPoints, healthPoints + amount);
        healthSlider.value = healthPoints;
    }

    void ReduceHealth(int amount)
    {
        healthPoints = Math.Max(0, healthPoints - amount);
        healthSlider.value = healthPoints;
    }

    void SetMaxHealth(int maxHealth)
    {
        maxHealth = Math.Max(1, maxHealth);
        healthSlider.maxValue = maxHealthPoints;
    }
}
