using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class ExtensionMethods
{
    public static int bulletLayerIgnored = ~(1 << LayerMask.NameToLayer("Ignore Bullets"));

    public static float Remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }


    public static float RandNegPos()
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

                                              //(float angle, Vector2 point, Vector2 pivot = new Vector2())
    public static Vector2 RotatePointAroundPivot(Vector2 point, Vector2 pivot, float angle) //https://stackoverflow.com/questions/2259476/rotating-a-point-about-another-point-2d
    {
        float s = Mathf.Sin(Mathf.Deg2Rad * angle);
        float c = Mathf.Cos(Mathf.Deg2Rad * angle);

        float xnew = point.x * c - point.y * s;
        float ynew = point.x * s + point.y * c;

        return new Vector2(xnew, ynew);
    }

    public static int[] Shuffle(int[] array)//object[] shuffle(object[] array)
    {
        for (int t = 0; t < array.Length; t++)
        {
            int tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        return array;
    }

    public static Color[] Shuffle(Color[] array)//object[] shuffle(object[] array)
    {
        for (int t = 0; t < array.Length; t++)
        {
            Color tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        return array;
    }

    public static List<GameObject> Shuffle(List<GameObject> array)//object[] shuffle(object[] array)
    {
        for (int t = 0; t < array.Count; t++)
        {
            GameObject tmp = array[t];
            int r = Random.Range(t, array.Count);
            array[t] = array[r];
            array[r] = tmp;
        }
        return array;
    }
    /*
    public static object[] shuffle(object[] array)
    {
        for (int t = 0; t < array.Length; t++)
        {
            object tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        return array;
    }


    public static T[] shuffle<T>(T[] array)//object[] shuffle(object[] array)
    {
        for (int t = 0; t < array.Length; t++)
        {
            T tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        return array;
    }

    public static T[] LastItem<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }*/
}

