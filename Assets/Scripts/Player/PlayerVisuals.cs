using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private float dashUsedBrigthness = 0.5f;
    //[SerializeField] private float velocityDeform = 0.5f;


    private Color mainColor;
    private bool isBlinking;
    private IEnumerator coroutine;
    private Rigidbody2D rb;
    private Tween shakeTween;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        
    }


    public void ShakeScale(float time, float strength)
    {
        /*
         ////obj.DOKill();
        //obj.GetComponentInChildren<PlayerVisuals>().transform.localScale = Vector3.one;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOShakeScale(time, strength));
        // seq.AppendCallback(() => obj.localScale = obj.GetComponent<PlayerMovement>().orgScale); //optimize this
        seq.AppendCallback(() => transform.localScale = Vector3.one); */
        

        if (shakeTween != null && shakeTween.IsPlaying())
        {
            shakeTween.Kill();
            //obj.GetComponentInChildren<PlayerVisuals>().transform.localScale = Vector3.one;
        }

        shakeTween = transform.DOShakeScale(time, strength);
    }

    private void LateUpdate()
    {
        // PAUSE THIS WHEN SCALE PLAYER BY DAMAGE (func below)
        if (shakeTween == null || !shakeTween.IsPlaying())//(!DOTween.IsTweening(transform))
        {
            float velocityDeform = 0.03f;

            var locVel = transform.InverseTransformDirection(rb.velocity);
            var deform = locVel;//.normalized;
            deform *= velocityDeform;

            deform.x = Mathf.Abs(deform.x);
            deform.y = Mathf.Abs(deform.y);

            transform.localScale = Vector3.one + deform;
            //visuals.localScale = Vector2.one + new Vector2(transform.up.x * deform.x, transform.up.y * deform.y);
        }
    }

    public void InitColor(Color col)
    {
        SetColorAll(col);
        mainColor = col;
    }


    public void SetActionOnCooldownCol()
    {
        if(!isBlinking)
            SetColorAll(ExtensionMethods.turnTeamColorDark(mainColor, dashUsedBrigthness));
    }
    public void SetMainColor()
    {
        if (!isBlinking)
            SetColorAll(mainColor);
    }


    public void blinkWhite(Color col, int frames)
    {
        //StopAllCoroutines();
        if (coroutine!=null) StopCoroutine(coroutine);
        StartCoroutine(coroutine = blink(col, frames));
    }



    private IEnumerator blink(Color col, int frames)
    {
        isBlinking = true;
        SetColorAll(col);

        //yield return new WaitForEndOfFrame();
        for (int i = 0; i < frames; i++)
            yield return null;

        SetColorAll(mainColor);
        isBlinking = false;
    }

    private void SetColorAll(Color col)
    {
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.color = col;
    }
}
