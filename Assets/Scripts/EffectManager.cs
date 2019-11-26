using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class EffectManager : MonoBehaviour
{
    //[SerializeField] private Sprite rect;
    [SerializeField] private GameObject deathExplosion;
    [SerializeField] private GameObject respawnAnim;
    [SerializeField] private GameObject dashPartic;
    [SerializeField] private GameObject bulletCrumblePartic;
    [SerializeField] private GameObject meleeCrumblePartic;
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

    private float trauma;
    private float shake;
    private Cinemachine.CinemachineBasicMultiChannelPerlin _perlin;

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<SpriteRenderer>();
        _perlin = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }


    public void DoDashPartic(Vector2 pos, Vector2 playerRot)
    {
        var exp = Instantiate(dashPartic, pos, Quaternion.identity).transform;
        exp.forward = -playerRot;
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

    public void playerDeathExplosion(Vector2 pos)
    {
        Instantiate(deathExplosion, pos, Quaternion.identity);
    }

    public void bulletDeathPartic(Vector2 hitPos, Transform bullet)
    {
        Instantiate(bulletCrumblePartic, hitPos, Quaternion.Euler(bullet.rotation.eulerAngles.z + 90, /*-90*/ -90, 0)); //rotate to look in opposite direction of bullet
    }

    public void meleeBulletDeathPartic(Vector2 hitPos, Transform bullet)
    {
        Instantiate(meleeCrumblePartic, hitPos, Quaternion.Euler(0, 0, bullet.rotation.eulerAngles.z)); //rotate to look in opposite direction of bullet

        // TODO: rather in hit normal of wall? well, is trigger.. or just in the middle of the melee bullet instead of hitPoint?
    }
    public void meleeBulletDeathPartic(Vector2 hitPos, Vector2 normal)
    {
        float rot_z = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;

        Instantiate(meleeCrumblePartic, hitPos, Quaternion.Euler(0f, 0f, rot_z - 90));
    }

    public void muzzle(Transform bullet, GameObject owner)
    {
        var m = Instantiate(muzzleFlashes[Random.Range(0, muzzleFlashes.Length)], bullet.transform.position, Quaternion.Euler(0, 0, bullet.rotation.eulerAngles.z)).transform;

        m.parent = owner.transform;

        foreach (SpriteRenderer spr in m.GetComponentsInChildren<SpriteRenderer>())
            spr.DOFade(0, muzzleFlashDur);
        // TODO: maybe also move individual muzzles in local up?

        Sequence seq = DOTween.Sequence();
        seq.Append(m.DOPunchScale(Vector3.one * muzzleScale, muzzleFlashDur));
        seq.AppendCallback(() => Destroy(m.gameObject));

    }





    // TODO: make shake 2D again, so can direct it
    public void AddShake(float str)
    {
        trauma += str;
    }

    /*public void AddShake(Vector2 dir, float strength) //dir is only 1,0  0,-1  1,1 etc
    {
        // Makes sure always 1 or 0 (?)
        if (dir.x < 0) dir.x = -dir.x;
        if (dir.x > 0) dir.x = 1;
        if (dir.y < 0) dir.y = -dir.y;
        if (dir.y > 0) dir.y = 1;
        trauma = new Vector2(trauma.x + (dir.x * strength), trauma.y + (dir.y * strength));
    }*/

    // Update shake
    private void Update()
    {
        trauma -= traumaFallOff * Time.deltaTime;
        trauma = Mathf.Clamp(trauma, 0, 1);
        shake = Mathf.Pow(trauma, powerOfAllShakes);

        shake *= maxoffset;




        for (var i = gridLights.Count - 1; i > -1; i--)
        {
            if (gridLights[i].centerTransform == null)
                gridLights.RemoveAt(i);
        }

        // doesnt work in late update bcz shader?
        if (gridLights.Count > 0)
        {
            //could also use a float array and just do j+=2 in shader
            List<Vector4> list = new List<Vector4>();

            // Make grid brigth where stuff is
            for (int i = 0; i < gridLights.Count; i++)
            {
                //check if the objects are still existent / enabled
                if (!gridLights[i].spriteRend.enabled) //TODO: look at brigthnes of spriterenderer
                    continue;

                list.Add(gridLights[i].centerTransform.position);
            }

            Vector4[] array = list.ToArray();
            grid.material.SetInt("maxLightingObjects", array.Length);
            grid.material.SetVectorArray("lightingObjects", array);
        }
        else
        {
            grid.material.SetInt("maxLightingObjects", 0);
        }
    }

    private void LateUpdate()
    {
        // Show shake
        _perlin.m_AmplitudeGain = shake;
        _perlin.m_FrequencyGain = frequency;
        // if too smooth, use cinemachine camera offset and move it manually



        //foreach(SpriteRenderer s in objectsLigthGrid)
        //    if (!s) objectsLigthGrid.Remove(s);
        //objectsLigthGrid = objectsLigthGrid.Where(item => item != null).ToList();
        /*for(var i = objectsLigthGrid.Count - 1; i > -1; i--)
        {
            if (objectsLigthGrid[i] == null)
            {
                objectsLigthGrid.RemoveAt(i);
                objectsLigthCenters.RemoveAt(i);
            }
        }

        // doesnt work in late update bcz shader?
        if (objectsLigthGrid.Count > 0)
        {
            //Vector4[] array = new Vector4[objectsLigthGrid.Count];
            List<Vector4> list = new List<Vector4>();

            // Make grid brigth where players are... and bullets too?
            for (int i = 0; i < objectsLigthGrid.Count; i++)
            {
                //check if the objects are still existent / enabled
                if (!objectsLigthGrid[i].enabled)
                    continue;

                //Vector4 localPoint = transform.InverseTransformPoint(objectsLigthGrid[i].transform.root.position); //needed?
                //localPoint.Scale(transform.localScale);
                //localPoint.w = 1;

                ////https://www.alanzucconi.com/2016/01/27/arrays-shaders-heatmaps-in-unity3d/
                //grid.material.SetVector("lightingObjects" + i.ToString(), localPoint);

                //https://stackoverflow.com/questions/45098671/how-to-define-an-array-of-floats-in-shader-properties
                //array[i] = localPoint;
                //list.Add(localPoint);

                //list.Add(objectsLigthGrid[i].transform.root.position);
                //print(objectsLigthGrid[i].transform.root.name + " " + objectsLigthGrid[i].transform.root.position);
                list.Add(objectsLigthCenters[i].position);
                print(objectsLigthCenters[i].name + " " + objectsLigthGrid[i].transform.position);
            }
            Vector4[] array = list.ToArray();
            grid.material.SetInt("maxLightingObjects", array.Length);
            grid.material.SetVectorArray("lightingObjects", array);
        }
        else
        {
            grid.material.SetInt("maxLightingObjects", 0);
        }*/


       
    }

    public void addGridLigth(SpriteRenderer s, Transform t)
    {
        GridLigth l = new GridLigth(s, t);
        gridLights.Add(l);
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
        if (paused)
            return;

        Time.timeScale = timeScale;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        paused = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        paused = false;
    }
}
