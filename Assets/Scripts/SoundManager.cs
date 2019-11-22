using SynchronizerData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    private AudioSource music;
    private BeatObserver beatObserver;
    private VolumeObserver volume;
    private BeatSynchronizer synchronizer;
    private PostProcessVolume postProcess;

    public AudioClip menuClip;
    public AudioClip gameplayClip;
    public float bloomChangeSpeed = 1;
    public float minBloom = 2;
    public float maxBloom = 10;
    public float volumeBloomMaxMulti = 1.5f;

    private int beatCounter;
    private float beatTimer;
    private float bloom;


    void Start()
    {
        bloom = minBloom;

        music = GetComponent<AudioSource>();
        volume = GetComponent<VolumeObserver>();
        beatObserver = GetComponent<BeatObserver>();
        synchronizer = GetComponent<BeatSynchronizer>();
        SceneManager.sceneLoaded += SceneLoadeded;
    }

    // changed from one scene to another
    private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        // Regularly loaded into gameplay from character selection
        if (FindObjectOfType<GameStateManager>().state == GameStateManager.GameState.Ingame) //(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1")
        {
            music.clip = gameplayClip;
            //music.Play();
            synchronizer.init(music);

            postProcess = Camera.main.GetComponent<PostProcessVolume>();
        }
    }


    void Update()
    {
        if ((beatObserver.beatMask & BeatType.OnBeat) == BeatType.OnBeat)
        {
            //transform.position = beatPositions[beatCounter];
            //print(beatCounter);

            beatCounter = (++beatCounter == 2 ? 0 : beatCounter);
        }

        //use dotween?

        int dir = beatCounter * 2 - 1; //go from 0 and 1 to -1 and 1
        beatTimer += dir * Time.deltaTime * bloomChangeSpeed;

        beatTimer = Mathf.Clamp(beatTimer, 0, 1);
        bloom = ExtensionMethods.Remap(beatTimer, 0,1,minBloom,maxBloom);
    }

    void LateUpdate()
    {

        if(postProcess)
        {
            float saturation = 5f;

            Bloom bloomLayer = null;

            postProcess.profile.TryGetSettings(out bloomLayer);

            //print(volume.getVolume());
            float volAmp = ExtensionMethods.Remap(volume.getVolume(),0,0.25f,1, volumeBloomMaxMulti); //generatlly observed that 0.5 is the max but during drop the song is about 0.3

            bloomLayer.intensity.value = bloom * volAmp;
        }
    }
}
