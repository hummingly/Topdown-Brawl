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

    public void setAim(bool aim)
    {
        this.aim = aim;
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
        playerMovement = gameObject.GetComponentInParent<PlayerMovement>();
        // FIND WITH TAG ????
    }

    
    void Update()
    {
        if (aim)
            lineRenderer.enabled = true;
        else
            lineRenderer.enabled = false;
        if (playerMovement == null)
            print("NULL PLAYERMOVEMENT");
        //print(playerMovement.getLastRot());
        //Vector2 v = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerMovement.getLastRot().normalized);
        //Debug.DrawLine(transform.position, playerMovement.getLastRot().normalized, Color.green);
        //playerMovement.getLastRot().normalized * 10f
        // Debug.DrawLine(transform.position, hit.point, Color.green);
        Debug.DrawRay(transform.position, playerMovement.getLastRot().normalized, Color.green); //Desired look dir
        //print(hit.point);
        laserHit.position = hit.point;
        lineRenderer.SetPosition(0, transform.position); // first position
        lineRenderer.SetPosition(1, hit.point); // end point of laser
        /*
        lineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerMovement.getLastRot().normalized, out hit))
        {
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        } else
        {
            lineRenderer.SetPosition(1, playerMovement.getLastRot().normalized * maxRange);
        }*/
    }
}
