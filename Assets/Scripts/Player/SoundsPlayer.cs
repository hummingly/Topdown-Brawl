using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    public AudioSource respawnClip;
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

    public void damaged()
    {
        damagedClip.Play(); //or oneshot?
    }

    public void death()
    {
        //deathClip.time = 0.1f;
        deathClip.Play();
    }

    public void respawn()
    {
        respawnClip.Play();
    }
}
