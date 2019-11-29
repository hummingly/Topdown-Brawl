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
    public AudioSource bounceWallClip2; //also on any bounce? maybe volume depending on speed
    public AudioSource dashClip;

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

    public void dash()
    {
        dashClip.time = 0.2f;
        dashClip.Play();
    }

    public void bounceWall(float speed)
    {
        //print(speed);
        //play whichever bounce clip is free, if both free play one random
        int rand = Random.Range(0, 2);
        AudioSource toPlay = rand == 0 ? bounceWallClip : bounceWallClip2;
        if (toPlay.isPlaying)
            toPlay = rand == 1 ? bounceWallClip : bounceWallClip2;

        toPlay.volume = speed * 0.05f;
        toPlay.Play();
    }
}
