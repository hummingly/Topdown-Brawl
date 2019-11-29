using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    public AudioSource damagedClip;
    public AudioSource damagedMuchClip;
    public AudioSource deathClip;
    public AudioSource bounceWallClip; //also on any bounce? maybe volume depending on speed

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damaged()
    {
        damagedClip.Play(); //or oneshot?
    }

    public void Death()
    {
        //deathClip.time = 0.1f;
        deathClip.Play();
    }
}
