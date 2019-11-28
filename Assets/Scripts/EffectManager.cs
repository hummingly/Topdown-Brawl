﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;
using Kino;

public class EffectManager : MonoBehaviour
{
    //[SerializeField] private Sprite rect;
    [SerializeField] private GameObject spawnProtection;
    [SerializeField] private Image screenFill;
    [SerializeField] private GameObject deathExplosion;
    [SerializeField] private GameObject respawnAnim;
    [SerializeField] private GameObject dashPartic;
    [SerializeField] private GameObject bulletCrumblePartic;
    [SerializeField] private GameObject bigSparks;
    [SerializeField] private GameObject[] muzzleFlashes;
    [SerializeField] private float muzzleFlashDur = 0.1f;
    [SerializeField] private float muzzleScale = 2f;
    [Space]
    //[SerializeField] private float killShakeStr = 2f;
    //[SerializeField] private float killShakeVibrate = 2f;
    //[SerializeField] private float killShakeDur = 1f;

    [SerializeField] private float maxoffset = 2; //shake of 2 would be very brutal... (transform.x + 2)
    [SerializeField] private float traumaFallOff = 0.03333f;
    [SerializeField] private float powerOfAllShakes = 2;
    [SerializeField] private float frequency = 1;

    private SpriteRenderer grid;
    //private List<GameObject> objectsLigthGrid = new List<GameObject>();
    //private List<SpriteRenderer> objectsLigthGrid = new List<SpriteRenderer>();
    private List<GridLigth> gridLights = new List<GridLigth>();

    private bool paused;

    private bool roundRunning = true;

    //private float trauma;
    //private float shake;
    private CinemachineBasicMultiChannelPerlin _perlin;
    private CinemachineCameraOffset offset;
    private Vector2 trauma;
    private Vector2 shake;
    private Vector2 camOffset;

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<SpriteRenderer>();

        //maximum grid lights at one time
        Vector4[] array = new Vector4[1000];
        float[] arrayF = new float[1000];
        grid.material.SetVectorArray("lightingObjects", array);
        grid.material.SetFloatArray("highlightRanges", arrayF);
        grid.material.SetFloatArray("intensities", arrayF);

        _perlin = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        offset = FindObjectOfType<CinemachineCameraOffset>();
    }


    public void DoDash(Vector2 pos, Vector2 playerRot, Transform player)
    {
        var exp = Instantiate(dashPartic, pos, Quaternion.identity).transform;
        exp.forward = -playerRot;

        // squeeze in dash dir
        //Sequence seq = DOTween.Sequence();
        //seq.Append(player.GetComponentInChildren<PlayerVisuals>().transform.DOScale(new Vector2(0.5f,1.5f), 0.5f));
        ////seq.Append(player.GetComponentInChildren<PlayerVisuals>().transform.DOScale(Vector2.one + playerRot, 0.5f));
        //seq.AppendCallback(() => player.GetComponentInChildren<PlayerVisuals>().transform.localScale = Vector3.one);
        //just do scale based on velocity? so no need to hardcode dash
    }

    // for now on spawn, but maybe rather an explosion on kill? or big dmg  !!!!!!!!!!
    public void SquareParticle(Vector2 pos)
    {
        // a circle that (almost) starts at max size, is rotated and has a color, then a mask that is rotated starts in middle and gets bigger until the whole thing is a hole, then delete

        float maxSize = 6;
        Color col = Color.white;

        /*GameObject go = new GameObject("Explosion Test");
        go.transform.position = pos;
        go.transform.localScale = Vector3.one * maxSize;
        go.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        var spr = go.AddComponent<SpriteRenderer>();
        spr.sprite = rect;
        spr.color = col;

        spr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        spr.sortingOrder = 100 + Random.Range(0, 100); // TODO: not optimal, but unlikely to get the same layer twice

        GameObject goM = new GameObject("Mask");
        goM.transform.parent = go.transform;
        goM.transform.localPosition = Vector3.zero;
        goM.transform.localScale = Vector2.one * 0.5f;
        var mask = goM.AddComponent<SpriteMask>();
        mask.sprite = rect;
        mask.isCustomRangeActive = true;
        mask.frontSortingOrder = spr.sortingOrder;
        */

        //TODO: use an object pool instead of instantiating so much (or maybe at least prefab?)


        var exp = Instantiate(respawnAnim, pos, Quaternion.identity).transform;
        exp.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        var sort = 100 + Random.Range(0, 100);
        var sprite = exp.GetComponent<SpriteRenderer>();
        sprite.color = col;
        sprite.sortingOrder = sort; // TODO: not optimal, but unlikely to get the same layer twice, so keep a pool with individual layers
        exp.GetComponentInChildren<SpriteMask>().frontSortingOrder = sort;


        // don't start at max size
        exp.localScale = Vector2.one * maxSize * 0.5f;

        Sequence seq = DOTween.Sequence();
        seq.Append(exp.DORotate(new Vector3(0, 0, exp.eulerAngles.z * 3), 0.5f));
        seq.Join(exp.GetChild(0).DOScale(Vector2.one, 0.5f));
        seq.Join(exp.DOScale(Vector2.one * maxSize, 0.25f)); //TODO: add a lil overshoot
        seq.AppendCallback(() => Destroy(exp.gameObject));

        //TODO: add easing       
    }

    public void startSequence()
    {
        var digital = Camera.main.GetComponent<DigitalGlitch>();
        var analog = Camera.main.GetComponent<AnalogGlitch>();

        //whole screen black
        //start at much glitch (cant see bcz of black tho)
        screenFill.color = Color.black;
        digital.intensity = 0.2f;


        float mapSize = GameObject.FindGameObjectWithTag("MapBounds").transform.localScale.x;
        FindObjectOfType<CinemachineVirtualCamera>().enabled = false;

        //go to clear color and less glitch
        Sequence seqFillFade = DOTween.Sequence();
        seqFillFade.AppendInterval(0.25f);
        seqFillFade.Append(screenFill.DOFade(0, 1.5f).SetEase(Ease.InCubic));//OutCubic
        seqFillFade.Join(DOTween.To(() => digital.intensity, x => digital.intensity = x, 0.001f, 2).SetEase(Ease.InCubic));
        //seqFillFade.AppendCallback(() => playerMove());
        seqFillFade.InsertCallback(1.5f, () => playerSpawn()); //synced do music delay (HARDCODED)

        //TODO: replace fade with ZAP animation? and do coutndown, then "GO"

        Sequence seqCam = DOTween.Sequence();
        seqCam.Append(Camera.main.DOOrthoSize(10 * mapSize, 1.75f).SetEase(Ease.OutCubic));
        seqCam.AppendCallback(() => FindObjectOfType<CinemachineVirtualCamera>().enabled = true);

    }


    private void playerSpawn()
    {
        foreach (PlayerMovement p in FindObjectsOfType<PlayerMovement>())
        {
            p.enabled = true;

            //place players at spawn pos again and do anim
            p.GetComponentInChildren<PlayerVisuals>().Hide(false);
            SquareParticle(p.transform.position);
        }
    }



    private IEnumerator screenBlink(Color col, int frames)
    {
        screenFill.color = col;

        //yield return new WaitForEndOfFrame();
        for (int i = 0; i < frames; i++)
            yield return null;

        screenFill.color = Color.clear;
    }

    public void playerDeath(Vector2 pos, Gamepad gamepad = null)// int deviceId)
    {
        var e = Instantiate(deathExplosion, pos, Quaternion.identity).transform;
        //e.gameObject.AddComponent<GridLightAddon>().set(0.2f, 2f);

        if(gamepad != null)
        {
            //rumble(gamepad, 0.2f, 0.1f); //good for third party controller
            //rumble(gamepad, 0.2f, 0, 0.5f); //good for ps4 controller
            //rumble(gamepad, 0.2f, 0.5f, 0.5f); // just set both ? well still differences in controllers... maybe third party has only one motor so its just more vibration?

            rumble(gamepad, 0.2f, 0.75f, 0.75f);
        }

        StartCoroutine(screenBlink(Color.white, 2));
        AddShake(1f);
        Stop(0.05f);
    }



    public void bulletDeathPartic(Vector2 hitPos, Transform bullet)
    {
        var p = Instantiate(bulletCrumblePartic, hitPos, Quaternion.Euler(bullet.rotation.eulerAngles.z + 90, /*-90*/ -90, 0)); //rotate to look in opposite direction of bullet
        p.transform.localScale = bullet.localScale*1.5f;
    }

    public void damagedEntity(Vector2 hitPos, Vector2 normal, float dmg)
    {
        float rot_z = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;

        var p = Instantiate(bigSparks, hitPos, Quaternion.Euler(0f, 0f, rot_z - 90));
        p.transform.localScale *= ExtensionMethods.Remap(dmg, 10, 50, 0.25f, 1.5f); //not working because particle system scale not changing
    }

    public void gotDamaged(Transform player)
    {
        shakeScale(player, 0.1f, 0.75f);

        player.GetComponentInChildren<PlayerVisuals>().blinkWhite(Color.white, 1);


        //AddShake(0.3f);
    }



    public void meleeBlow(Transform owner)
    {
        shakeScale(owner, 0.1f, 0.75f);
    }

    public void snipeShot(Vector2 pos, Transform bullet, GameObject owner, Gamepad gamepad = null)
    {
        var p = Instantiate(bulletCrumblePartic, pos, Quaternion.Euler(bullet.rotation.eulerAngles.z - 90, -90, 0)); //look in shot dir
        p.transform.localScale *= 4;

        rumble(gamepad, 0.1f, 0.2f, 0.2f);

        shakeScale(owner.transform, 0.2f, 1f);
    }

    public void muzzle(float dmg, Transform bullet, GameObject owner)
    {
        var m = Instantiate(muzzleFlashes[Random.Range(0, muzzleFlashes.Length)], bullet.transform.position, Quaternion.Euler(0, 0, bullet.rotation.eulerAngles.z)).transform;

        m.parent = owner.transform;

        foreach (SpriteRenderer spr in m.GetComponentsInChildren<SpriteRenderer>())
            spr.DOFade(0, muzzleFlashDur);
        // TODO: maybe also move individual muzzles in local up?

        Sequence seq = DOTween.Sequence();
        seq.Append(m.DOPunchScale(Vector3.one * muzzleScale * bullet.localScale.y, muzzleFlashDur));
        seq.AppendCallback(() => Destroy(m.gameObject));

        //addGridLigth(0.5f, 2f, m.GetComponentInChildren<SpriteRenderer>(), m);
        addGridLigth(dmg * 0.05f, 2f, m.GetComponentInChildren<SpriteRenderer>(), m); //TODO: muzzle transform not rly centerd...


        shakeScale(owner.transform, 0.05f, 0.5f);
    }


    public GameObject invincible(Transform player, float dur)
    {
        var p = Instantiate(spawnProtection, player.transform.position, Quaternion.identity);
        //fix it to them, and make it transparent
        p.transform.parent = player;

        Sequence seq = DOTween.Sequence();
        seq.Append(p.transform.DOScale(Vector3.one * 2, dur / 4));
        seq.Append(p.transform.DOScale(Vector3.one * 1.5f, dur / 4));
        seq.Append(p.transform.DOScale(Vector3.one * 2, dur / 4));
        seq.Append(p.transform.DOScale(Vector3.one * 1.5f, dur / 4));
        seq.Append(p.transform.GetComponent<SpriteRenderer>().DOFade(0, dur / 4));
        seq.AppendCallback(() => Destroy(p.gameObject));

        return p;
    }
    public void stopInvincible(GameObject p)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(p.transform.GetComponent<SpriteRenderer>().DOFade(0, 0.25f));
        seq.Join(p.transform.DOScale(Vector3.one * 3, 0.25f));
        seq.AppendCallback(() => Destroy(p.gameObject));
    }


    public float gameOver(Vector2 explosionPos)
    {
        float dur = 2;

        Instantiate(bigSparks, explosionPos, Quaternion.Euler(0f, 0f, 90));
        Instantiate(bigSparks, explosionPos, Quaternion.Euler(0f, 0f,-90));
        Instantiate(bigSparks, explosionPos, Quaternion.Euler(0f, 0f,180));
        Instantiate(bigSparks, explosionPos, Quaternion.Euler(0f, 0f,-180));

        roundRunning = false;
        Time.timeScale = 0.1f;

        // get normal speed again
        //StartCoroutine(speedUpTime());
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, dur).SetEase(Ease.InQuad).SetUpdate(true);

        //TODO: also zoom in on position
        //FindObjectOfType<Cinemachine.CinemachineTargetGroup>().RemoveMember();
        var heavyPlaceholder = new GameObject("Zoom on dis").transform;
        heavyPlaceholder.position = explosionPos;
        FindObjectOfType<Cinemachine.CinemachineTargetGroup>().AddMember(heavyPlaceholder, 10, 5);

        //either go to timescale 0 again, or disable all input
        FindObjectOfType<PlayerSpawner>().Disable();
        foreach (BotTest b in FindObjectsOfType<BotTest>())
            b.enabled = false;
        foreach (PlayerMovement b in FindObjectsOfType<PlayerMovement>())
            b.enabled = false;
        foreach (Skill b in FindObjectsOfType<Skill>())
            b.enabled = false;

        return dur;
    }




    // WILL ONLY WORK TO SCALE PLAYERS ATM
    private void shakeScale(Transform obj, float time, float strength)
    {
        /*obj.DOKill(); //prevent overlap or staying deformation
        Sequence seq = DOTween.Sequence();
        seq.Append(obj.DOShakeScale(time, strength));
        seq.AppendCallback(() => obj.DOKill()); //prevent overlap or staying deformation*/

        obj.GetComponentInChildren<PlayerVisuals>().ShakeScale(time, strength);
        /*
        //obj.DOKill();
        //obj.GetComponentInChildren<PlayerVisuals>().transform.localScale = Vector3.one;
        Sequence seq = DOTween.Sequence();
        seq.Append(obj.GetComponentInChildren<PlayerVisuals>().transform.DOShakeScale(time, strength));
       // seq.AppendCallback(() => obj.localScale = obj.GetComponent<PlayerMovement>().orgScale); //optimize this
        seq.AppendCallback(() => obj.GetComponentInChildren<PlayerVisuals>().transform.localScale = Vector3.one); //optimize this
        */
    }





    public void AddShake(float strength, Vector2 dir = new Vector2(), float threshHold = 0) //dir only 1,0  0,-1  1,1  etc
    {
        //print(trauma); //only do small shake on shooting if no other shakes active
        //if (threshHold != 0 && threshHold <= trauma.magnitude)
        //    return;
        if (threshHold != 0) return; //nvm, still stacks very bad


        if (dir == Vector2.zero)
            dir = Random.insideUnitCircle;
        //only 0/1 values since -1/1 is done below
        if (dir.x < 0) dir.x = -dir.x;
        if (dir.x > 0) dir.x = 1;
        if (dir.y < 0) dir.y = -dir.y;
        if (dir.y > 0) dir.y = 1;
        trauma = new Vector2(trauma.x + (dir.x * strength), trauma.y + (dir.y * strength));
    }

    // Update shake
    private void Update()
    {
        trauma.x -= traumaFallOff * Time.deltaTime;
        trauma.x = Mathf.Clamp(trauma.x, 0, 1);
        shake.x = Mathf.Pow(trauma.x, powerOfAllShakes);

        trauma.y -= traumaFallOff * Time.deltaTime;
        trauma.y = Mathf.Clamp(trauma.y, 0, 1);
        shake.y = Mathf.Pow(trauma.y, powerOfAllShakes);

        camOffset = new Vector2(shake.x * (maxoffset * Random.Range(-1f, 1f)), shake.y * (maxoffset * Random.Range(-1f, 1f)));
    }

    private void LateUpdate()
    {
        // Show shake
        //_perlin.m_AmplitudeGain = shake;
        //_perlin.m_FrequencyGain = 11 - shake;//frequency;
        // frequency should maybe be less and less the stronger the shake?
        // if too smooth, use cinemachine camera offset and move it manually
        // -> cinemachine noise didn't seem to give enough control, overlapping shakes etc dunno

        offset.m_Offset = camOffset;
        //print(offset.m_Offset + " " + camOffset);



        // Light on grid (TODO: actualy do real shadows with rays)
        for (var i = gridLights.Count - 1; i > -1; i--)
        {
            if (gridLights[i].centerTransform == null)
                gridLights.RemoveAt(i);
        }

        foreach(GridLigth g in gridLights)
            g.updateIntensity();
    
        if (gridLights.Count > 0)
        {
            //could also use a float array and just do j+=2 in shader, for less memory
            List<Vector4> listPos = new List<Vector4>();
            List<float> listRan = new List<float>();
            List<float> listInt = new List<float>();

            // Make grid brigth where stuff is
            for (int i = 0; i < gridLights.Count; i++)
            {
                //check if the objects are still existent / enabled
                if (!gridLights[i].spriteRend.enabled) //TODO: look at brigthnes of spriterenderer
                    continue;

                listPos.Add(gridLights[i].centerTransform.position);
                listRan.Add(gridLights[i].range);
                listInt.Add(gridLights[i].intensity);
            }

            Vector4[] arrayPos = listPos.ToArray();
            float[] arrayRan = listRan.ToArray();
            float[] arrayInt = listInt.ToArray();
            grid.material.SetInt("maxLightingObjects", arrayPos.Length);
            grid.material.SetVectorArray("lightingObjects", arrayPos);
            grid.material.SetFloatArray("highlightRanges", arrayRan);
            grid.material.SetFloatArray("intensities", arrayInt);
        }
        else
        {
            grid.material.SetInt("maxLightingObjects", 0);
        }
    }

    public GridLigth addGridLigth(float i, float r, SpriteRenderer s, Transform t)
    {
        GridLigth l = new GridLigth(i, r, s, t);
        gridLights.Add(l);
        return l;
    }

    /*
    public void playerShotShake()
    {
        StartCoroutine(ShakeCamera(1, 1, 0.1f));
    }

    public void playerHitShake()
    {
        StartCoroutine(ShakeCamera(2, 2, 0.2f));
    }

    public void playerDeathShake()
    {
        StartCoroutine(ShakeCamera(10,10,0.25f));
    }

    public void gameOverShake()
    {
        StartCoroutine(ShakeCamera(20,20,0.5f));
    }

    private IEnumerator ShakeCamera(float amp, float freq, float time)
    {
        _perlin.m_AmplitudeGain = amp;
        _perlin.m_FrequencyGain = freq;
        yield return new WaitForSeconds(time);
        CameraReset();
        //TODO: if not smooth enough reset, use trauma ... NVM need it anway to stack shakes
    }

    private void CameraReset()
    {
        _perlin.m_AmplitudeGain = 0;
        _perlin.m_FrequencyGain = 0;
    }*/


    public void Stop(float duration)
    {
        Stop(duration, 0.0f);
    }

    public void Stop(float duration, float timeScale)
    {
        if (paused ||!roundRunning)
            return;

        Time.timeScale = timeScale;
        StartCoroutine(Wait(duration));
    }

    private IEnumerator Wait(float duration)
    {
        paused = true;
        yield return new WaitForSecondsRealtime(duration);
        if(roundRunning) Time.timeScale = 1.0f;
        paused = false;
    }



    private void rumble(Gamepad gamepad, float fallOfDur, float startLow = 0, float startHigh = 0)
    {
        //Gamepad.all[device].SetMotorSpeeds(startLow, startHight);

        float amp = ExtensionMethods.getGamepadAmp(gamepad);
        StartCoroutine(rumbleFor(gamepad, fallOfDur, startLow * amp, startHigh * amp));
    }

    private IEnumerator rumbleFor(Gamepad gamepad, float fallOfDur, float startLow , float startHigh)
    {
        // TODO: maybe set less rumble over time? currently on/off

        gamepad.SetMotorSpeeds(startLow, startHigh);

        yield return new WaitForSecondsRealtime(fallOfDur);

        //gamepad.SetMotorSpeeds(0, 0);
        gamepad.ResetHaptics();
    }
}
