using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectManager : MonoBehaviour
{
    //[SerializeField] private Sprite rect;
    [SerializeField] private GameObject explosionTest;
    [SerializeField] private GameObject dashPartic;
    [SerializeField] private GameObject bulletCrumblePartic;
    [SerializeField] private GameObject[] muzzleFlashes;
    [SerializeField] private float muzzleFlashDur = 0.1f;
    [SerializeField] private float muzzleScale = 2f;


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


        var exp = Instantiate(explosionTest, pos, Quaternion.identity).transform;
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

    public void bulletDeathPartic(Vector2 hitPos, Transform bullet)
    {
        Instantiate(bulletCrumblePartic, hitPos, Quaternion.Euler(bullet.rotation.eulerAngles.z + 90, /*-90*/ -90, 0)); //rotate to look in opposite direction of bullet
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
}
