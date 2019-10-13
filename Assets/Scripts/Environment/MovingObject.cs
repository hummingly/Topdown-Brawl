using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingObject : MonoBehaviour
{
    private enum MoveType { PingPong, PingPongBounce, Circle };
    private enum AnimationTypes { None, Rotate, RotateAndScale };

    [SerializeField] private MoveType movement = MoveType.PingPong;
    [SerializeField] private Vector3 direction;
    //[SerializeField] private float startDist = 0.5f; //TODO: implement start at the middle of ping pong time, or at one end
    [SerializeField] private float speed = 1f;
    [SerializeField] private float distance; //before turn around

    [Space]
    [Header("Anim")]

    [SerializeField] private AnimationTypes anim = AnimationTypes.None;
    [SerializeField] private float rotateSpeed = 1;

    private Rigidbody2D rb;    // TODO: Rather use dotween? Can work with rb too!!!
    private Vector3 startPos;


    void Start()
    {
        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
        }
        startPos = transform.position;

        //if(anim == AnimationTypes.Rotate)
        //    transform.DORotate(new Vector3(0, 0, transform.rotation.z + 180), rotateSpeed).SetEase(Ease.Linear).SetLoops(-1); // StartCoroutine(startRotAnim());

        if(movement == MoveType.PingPong)
        {
            Sequence seq = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo);
            seq.Append(rb.DOMove(transform.position + direction * distance, speed));
            seq.AppendInterval(0.5f);
            seq.Append(rb.DOMove(transform.position - direction * distance, speed));
            seq.AppendInterval(0.5f);
            //seq.Append(rb.DOMove(transform.position - direction * distance / 2, speed));
        }
        
        if (movement == MoveType.PingPongBounce)
        {
            transform.position = transform.position - direction * distance;
            Sequence seq = DOTween.Sequence().SetLoops(-1, LoopType.Restart);
            seq.Append(rb.DOMove(transform.position + direction * distance * 2, speed).SetEase(Ease.OutBounce));
            seq.AppendInterval(0.5f);
            seq.Append(rb.DOMove(transform.position, speed).SetEase(Ease.OutBounce));
            seq.AppendInterval(0.5f);
            //seq.Append(rb.DOMove(transform.position + direction * distance, speed).SetEase(Ease.OutBounce));
            //seq.AppendInterval(0.5f);
            //seq.Append(rb.DOMove(transform.position - direction * distance, speed).SetEase(Ease.OutBounce));
            //seq.AppendInterval(0.5f);
            //seq.Goto(0.5f, true);
        }
        /**/
    }


    private void FixedUpdate()
    {
        //float dist = Vector3.Distance(startPos, transform.position);

        //transform.position += direction * Time.deltaTime * speed; //* (distance-dist);
        /*rb.MovePosition(transform.position + direction * Time.deltaTime * speed); //* (distance-dist);

        float dist = Vector3.Distance(startPos, transform.position);

        if (dist >= distance)
        {
            direction = -direction;
            rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
        }*/
    }


    public void Reset()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0;
        }
    }


    private void LateUpdate()
    {
        if (anim == AnimationTypes.Rotate)
            transform.Rotate(new Vector3(0, 0, (rotateSpeed) * Time.deltaTime));//transform.DORotate();

        //for (int i = 0; i < layers.Length; i++)
        //   layers[i].transform.Rotate(new Vector3(0, 0, (rotateSpeed /* + Random.Range(0, rotateSpeedRandMore)*/) * Time.deltaTime));
    }


    /*private IEnumerator startRotAnim()
    {
        //visuals.transform.DORotate(new Vector3(0,0,transform.rotation.z + 1), rotateSpeed).SetLoops(-1);
        //visuals.transform.DOShakeScale(scaleDur, scaleStr, scaleVib, scaleRand); //SetLoops(-1)
        //visuals.transform.DOPunchScale(punchVec, scaleDur, scaleVib, scaleElast).SetLoops(-1);


        // for multiple rot layers:
        //for (int i = 0; i < layers.Length; i++)
        //{
            //Sequence seq = DOTween.Sequence().SetLoops(-1);
            //seq.Append(layers[i].transform.DOScale(layers[i].transform.localScale * scaleDivider, scaleShrinkDur).SetEase(Ease.InOutSine));
            //seq.Append(layers[i].transform.DOScale(layers[i].transform.localScale, expandDur).SetEase(Ease.Linear));

            //yield return new WaitForSeconds(scaleOffset);
        //}
    }*/


    public Vector2 getMoveDir()
    {
        //return rb.velocity; won't work since using movePosition

        return direction;
    }
}
