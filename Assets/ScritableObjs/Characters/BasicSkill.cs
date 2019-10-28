using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Skill", menuName = "Basic Skill")]
public class BasicSkill : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite sprite;
    public GameObject projectile;

    public float speed;
    public float cooldown;
    public int damage;
    public float range;

    // implement range!!!!!!!!!!!!!!!!!!!!
}
