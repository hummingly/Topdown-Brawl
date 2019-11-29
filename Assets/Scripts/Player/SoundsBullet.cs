using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsBullet : MonoBehaviour
{
    public AudioSource shotClip;
    public AudioSource hitNeutralClip;
    public AudioSource hitEnemyClip;

    void Start()
    {

    }

    void Update()
    {

    }

    public void shot()
    {
        shotClip.Play();
    }

    public void hitNeutral()
    {
        //print("neutral");
        hitNeutralClip.Play();
    }

    public void hitEnemy()
    {
        //print("e");
        hitEnemyClip.Play();
    }
}
