using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Role role;

    void Start()
    {
        if (role != null)
        {
            
            BasicSkill basic = role.basic;
            if (basic != null)
                gameObject.AddComponent<Launcher>();
                
            

            SecondarySkill secondary = role.secondary;
            if (secondary != null)
                gameObject.AddComponent<Launcher>();


            Launcher[] launchers = gameObject.GetComponents<Launcher>();

            launchers[0].Init(basic.projectile, Launcher.Purpose.Basic, basic.speed, basic.cooldown, basic.damage);
            launchers[1].Init(secondary.projectile, Launcher.Purpose.Secondary, secondary.speed, secondary.cooldown, secondary.damage);

        }
    }
}
