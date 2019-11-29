using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsBullet : MonoBehaviour
{
    public AudioSource shotClip;
    public AudioSource hitNeutralClip;
    public AudioSource hitEnemyClip;

    public GameObject bullet;

    void Start()
    {
        bullet = transform.parent.gameObject;

        GetComponentInChildren<SoundsBullet>().transform.parent = null; //un-parent so sound doesnt stop if bullet destroyed, can be done here already cause no 3D sound?
    }

    void Update()
    {
        if (!bullet && !shotClip.isPlaying && !hitNeutralClip.isPlaying && !hitEnemyClip.isPlaying)
            Destroy(gameObject);
    }

    public void shot()
    {
        //print("shot");
        shotClip.Play();
    }

    public void HitNeutral()
    {
        //print("neutral");
        hitNeutralClip.Play();
    }

    public void HitEnemy()
    {
        //print("enemy");
        hitEnemyClip.Play();
    }
}
