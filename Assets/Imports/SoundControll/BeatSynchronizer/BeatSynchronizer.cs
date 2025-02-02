﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This class should be attached to the audio source for which synchronization should occur, and is 
/// responsible for synching up the beginning of the audio clip with all active beat counters and pattern counters.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BeatSynchronizer : MonoBehaviour {

	public float bpm = 120f;		// Tempo in beats per minute of the audio clip.
	public float startDelay = 0f;	// Number of seconds to delay the start of audio playback.
	public delegate void AudioStartAction(double syncTime);
	public static event AudioStartAction OnAudioStart;
    double initTime = AudioSettings.dspTime;

    //private float startDelay = 0;

    void Start ()
	{
		initTime = AudioSettings.dspTime;

		GetComponent<AudioSource>().PlayScheduled(initTime + startDelay);
		if (OnAudioStart != null) {
			OnAudioStart(initTime + startDelay);
		}
	}

    /*
    private void Update()
    {
        startDelay += Time.deltaTime; //= AudioSettings.dspTime;
    }

    public void init(AudioSource audio)
    {
        audio.PlayScheduled(initTime + startDelay);
        if (OnAudioStart != null)
        {
            OnAudioStart(initTime + startDelay);
        }
        

        //audio.Play();
        //if (OnAudioStart != null)
        //{
            //double startDelay = AudioSettings.dspTime;
            //audio.PlayScheduled(initTime + startDelay);
            //OnAudioStart(initTime + startDelay);
            
            //audio.PlayScheduled(initTime);
            //OnAudioStart(initTime);

        //    OnAudioStart(0);
        //}
    }*/

}
