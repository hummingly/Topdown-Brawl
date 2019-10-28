using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Role : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite sprite;

    public enum Genre { Tank, Dps, etc };
    public Genre genre;
    public Sprite roleIcon;

    public BasicSkill basic;
    public SecondarySkill secondary;
    //public Skill special;
}
