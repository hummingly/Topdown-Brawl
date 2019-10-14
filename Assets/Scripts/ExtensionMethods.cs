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


    /*public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) //https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }*/

    public static Vector2 RotatePointAroundPivot(Vector2 point, Vector2 pivot, float angle) //https://stackoverflow.com/questions/2259476/rotating-a-point-about-another-point-2d
    {
        float s = Mathf.Sin(Mathf.Deg2Rad * angle);
        float c = Mathf.Cos(Mathf.Deg2Rad * angle);

        float xnew = point.x * c - point.y * s;
        float ynew = point.x * s + point.y * c;

        return new Vector2(xnew, ynew);
    }
}

