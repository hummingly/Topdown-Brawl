using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform laserHit;

    private PlayerMovement playerMovement;
    private bool aim;
    //[SerializeField] private int maxRange;

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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerMovement.GetLastRot().normalized, float.MaxValue, bulletLayerIgnored);
        //Debug.DrawLine(transform.position, playerMovement.getLastRot().normalized, Color.green);
        //Debug.DrawLine(transform.position, hit.point, Color.green);

        if (hit)
        {
            laserHit.position = hit.point;
        }

        lineRenderer.SetPosition(0, transform.position); // first position
        lineRenderer.SetPosition(1, laserHit.position);//hit.point); // end point of laser
    }
}
