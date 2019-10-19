using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;


    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                if(ps.transform.parent != null)
                    Destroy(ps.transform.parent.gameObject);
                else
                    Destroy(gameObject);

            }
        }
    }
}