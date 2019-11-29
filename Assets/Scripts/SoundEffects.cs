using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip startLevelClip;
    public AudioClip respawnClip;
    public AudioClip gameOverClip;

    [Space]
    public AudioSource levelLoad;
    public AudioSource gameOverBass;
    //public AudioSource spawnSound;//maybe rather have this on each player?



    void Start()
    {
        //bigEvents.clip = startLevelClip;
        levelLoad.PlayOneShot(startLevelClip);
    }


    void Update()
    {
        
    }

    public void restart()
    {
        //bigEvents.PlayOneShot(startLevelClip); start clip reversed
        /*bigEvents.timeSamples = bigEvents.clip.samples - 1;
        bigEvents.pitch = -1;
        bigEvents.PlayOneShot(startLevelClip);*/

        /*bigEvents.time = startLevelClip.length;
        bigEvents.pitch = -1;
        bigEvents.clip = startLevelClip;
        bigEvents.Play();*/
        levelLoad.loop = true; //weird workaround, setting time to 1 should work
        levelLoad.pitch = -1;
        levelLoad.clip = startLevelClip;
        levelLoad.Play();
    }

    public void gameOver()
    {
        gameOverBass.time = 0.1f;
        gameOverBass.Play();
        levelLoad.PlayOneShot(gameOverClip);
        FindObjectOfType<SoundManager>().slowDownThenFadeMusic();

    }
}
