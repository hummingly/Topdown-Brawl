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
    public BeatObserver beatObserverCounter;
    public BeatObserver beatObserverPattern;
    public int[] patternTest = new int[] { 0,1,1,1,0,0,1,1,1,1,0,0};
    private VolumeObserver volume;
    private BeatSynchronizer synchronizer;
    private PostProcessVolume postProcess;

    //public AudioClip menuClip;
    //public AudioClip gameplayClip;
    public float bloomChangeSpeed = 1;
    public float minBloom = 2;
    public float maxBloom = 10;
    public float volumeBloomMaxMulti = 1.5f;
    public Vector2 lensDist = new Vector2(20,30);
    public Vector2 chromAbrev = new Vector2(0.122f, 0.25f);
    public Vector2 saturation = new Vector2(0, 10);
    public Vector2 contrast = new Vector2(0, 10);

    private int beatCounter;
    private int beatCounterPattern = 0;
    private float beatTimer;
    private float bloom;

    public float dropStartTransSpd = 10;
    public float dropEndTransSpd = 5;
    private float dropTimer;
    private float drop;
    private float lens;
    private float colorAbr;
    private int dropDir;


    void Start()
    {
        bloom = minBloom;

        music = GetComponent<AudioSource>();
        volume = GetComponent<VolumeObserver>();
        //beatObserverCounter = GetComponent<BeatObserver>();
        synchronizer = GetComponent<BeatSynchronizer>();
        postProcess = Camera.main.GetComponent<PostProcessVolume>();
        //SceneManager.sceneLoaded += SceneLoadeded;
    }

    // changed from one scene to another
    /*private void SceneLoadeded(Scene scene, LoadSceneMode arg1)
    {
        // Regularly loaded into gameplay from character selection
        if (FindObjectOfType<GameStateManager>().state == GameStateManager.GameState.Ingame) //(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapNormal1")
        {
            //music.clip = gameplayClip;
            //music.Play();
            //synchronizer.init(music);

            postProcess = Camera.main.GetComponent<PostProcessVolume>();
        }
    }*/


    void Update()
    {
        //use dotween instead?

        //go from 0 and 1 to -1 and 1
        int dir = beatCounter * 2 - 1;
        beatTimer += dir * Time.deltaTime * bloomChangeSpeed;// * -1; //-1 needed to go from offbeat to onbeat (because of screne transition?)

        beatTimer = Mathf.Clamp(beatTimer, 0, 1);
        transform.position = new Vector3(beatTimer, 0, 0);
        bloom = ExtensionMethods.Remap(beatTimer, 0,1,minBloom,maxBloom);


        //print(dropDir);

        float dropChangeSpeed = 0;
        if (dropDir == 1)
            dropChangeSpeed = dropStartTransSpd;
        if (dropDir == -1)
            dropChangeSpeed = dropEndTransSpd;
        drop += dropDir * Time.deltaTime * dropChangeSpeed;
        drop = Mathf.Clamp(drop, 0, 1);

        lens = ExtensionMethods.Remap(drop, 0, 1, lensDist.x, lensDist.y);
        colorAbr = ExtensionMethods.Remap(drop, 0, 1, chromAbrev.x, chromAbrev.y);


        if ((beatObserverCounter.beatMask & BeatType.OnBeat) == BeatType.OnBeat)
        {
            //transform.position = beatPositions[beatCounter];
            //print(beatCounter);

            // This last
            beatCounter = (++beatCounter == 2 ? 0 : beatCounter);
        }
        if ((beatObserverPattern.beatMask & BeatType.OnBeat) == BeatType.OnBeat)
        {
            //print(beatCounterPattern + " " + patternTest[beatCounterPattern]);

            dropDir = patternTest[beatCounterPattern] * 2 - 1;    


            // This last
            beatCounterPattern = (++beatCounterPattern == patternTest.Length ? 0 : beatCounterPattern);
        }
    }

    void LateUpdate()
    {

        if(postProcess)
        {
            Bloom bloomLayer = null;
            ChromaticAberration chrom = null;
            LensDistortion lensLayer = null;
            ColorGrading colors = null;

            postProcess.profile.TryGetSettings(out bloomLayer);
            postProcess.profile.TryGetSettings(out chrom);
            postProcess.profile.TryGetSettings(out lensLayer);
            postProcess.profile.TryGetSettings(out colors);

            //print(volume.getVolume());
            float normVol = ExtensionMethods.Remap(volume.getVolume(), 0, 0.25f, 0, 1); //generatlly observed that 0.5 is the max but during drop the song is about 0.3
            //normVol = Mathf.Clamp(normVol, 0, 1); //this really limits to max, but without it feels better

            float bloomAmp = ExtensionMethods.Remap(normVol, 0, 1, 1, volumeBloomMaxMulti);
            //float colorAbr = ExtensionMethods.Remap(normVol, 0, 1, chromAbrev.x, chromAbrev.y);
            float sat = ExtensionMethods.Remap(normVol, 0, 1, saturation.x, saturation.y);
            float cont = ExtensionMethods.Remap(normVol, 0, 1, contrast.x, contrast.y);

            //chrom.intensity.value = colorAbr;
            bloomLayer.intensity.value = bloom * bloomAmp;
            colors.saturation.value = sat;
            colors.contrast.value = cont;

            /*
            //float lens = ExtensionMethods.Remap(normVol, 0, 1, lensDist.x, lensDist.y);
            float lens;
            if (normVol < 0.8f)
                lens = lensDist.x;
            else
                lens = lensDist.y;

            lensLayer.intensity.value = lens;*/

            lensLayer.intensity.value = lens;
            chrom.intensity.value = colorAbr;
        }
    }

    //private IEnumerator interpolate()

}
