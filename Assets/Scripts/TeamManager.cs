using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour // Singleton instead of static, so can change variable in inspector
{
    public static TeamManager instance = null;




    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

    }

}
