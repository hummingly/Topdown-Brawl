using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip startLevelClip;
    public AudioClip respawnClip;
    [Space]
    public AudioSource bigEvents; //stuff like lvl load, team won, etc that never happen at the same time
    //public AudioSource spawnSound;//maybe rather have this on each player?



    void Start()
    {
        //bigEvents.clip = startLevelClip;
        bigEvents.PlayOneShot(startLevelClip);
    }


    void Update()
    {
        
    }
}
