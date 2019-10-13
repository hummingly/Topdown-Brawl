using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float randNegPos()
    {
        return (Random.Range(0, 2) * 2) - 1;
    }

    /*public static float randNegPos(this float val)
    {
        return val * Random.Range(0, 2);
    }*/
}

