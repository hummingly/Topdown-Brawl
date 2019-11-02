using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public new string name;
    public string description;

    public GameObject prefab;
    public Sprite sprite;

    public enum Role { Tank, Dps, etc };
    public Role role;
    public Sprite roleIcon;

    //public Skill basic;
    //public Skill secondary;
    //public Skill special;
}
