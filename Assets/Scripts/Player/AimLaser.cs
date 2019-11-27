using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLaser : MonoBehaviour // TODO: extend to be a general range indication class
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform laserHit;
    [SerializeField] private float laserFadeAt = 0.9f;

    private PlayerMovement playerMovement;
    private bool aim;
    private float laserDist;
    //[SerializeField] private int maxRange;

    public void setDist(float dist)
    {
        this.laserDist = dist;
    }
    public void SetAim(bool aim)
    {
        this.aim = aim;
    }


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
        playerMovement = gameObject.GetComponentInParent<PlayerMovement>();
    }

    
    void Update()
    {
        if (aim)
            lineRenderer.enabled = true;
        else
            lineRenderer.enabled = false;

        var bulletLayerIgnored = ~(1 << LayerMask.NameToLayer("Ignore Bullets"));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerMovement.GetLastRot().normalized, laserDist, bulletLayerIgnored);
        //Debug.DrawLine(transform.position, playerMovement.getLastRot().normalized, Color.green);
        //Debug.DrawLine(transform.position, hit.point, Color.green);

        if (hit)
            laserHit.position = hit.point;
        else
            laserHit.position = (Vector2)transform.position + playerMovement.GetLastRot().normalized * laserDist;


        lineRenderer.SetPosition(0, transform.position); // first position
        lineRenderer.SetPosition(1, laserHit.position);//hit.point); // end point of laser


        //fade line at the tip, but if hit a wall before that dont fade it
        float reachedDist = Vector2.Distance(transform.position, laserHit.position);
        float percentage = reachedDist / laserDist;
        //laserFadeAt = Mathf.Clamp(laserFadeAt, 0, 0.999f);
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            lineRenderer.colorGradient.colorKeys ,
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, laserFadeAt + 1 - percentage), new GradientAlphaKey(0, 1) }
        );
        lineRenderer.colorGradient = gradient;

        //TODO: make more smooth, still a bit sloppy when going from looking at wall to aiming to nowhere
    }
}
