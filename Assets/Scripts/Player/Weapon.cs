using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private DamagingArea melee;

    // ATTEMPT FOR A CLSOE RANGE ATTACK

    void Awake()
    {
        melee = GetComponentInChildren<DamagingArea>();
    }

    void Update()
    {
        
    }

    private void OnZRightTrigger()
    {
        // first just use box collider, but later more accurate with polygon? or circle and check angle ??????
        // OR rather raycast so can't go through wall
        // based on velocity

        melee.activate();
    }
}
