using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ExtensionMethods
{
    //public static bool useTweening = true;
    //public static bool useParticles = false;

    //public static bool doShoot = true;
    //public static bool getDamaged = true;

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

    public static IList<T> Shuffle<T>(IList<T> collection)
    {
        int seed = UnityEngine.Random.Range(0, 1000);
        return Shuffle(collection, seed);
    }

    public static IList<T> Shuffle<T>(IList<T> collection, int seed)
    {
        Random.InitState(seed);
        for (int t = 0; t < collection.Count; t++)
        {
            T tmp = collection[t];
            int r = Random.Range(t, collection.Count);
            collection[t] = collection[r];
            collection[r] = tmp;
        }
        return collection;
    }

    public static float getGamepadAmp(Gamepad gamepad)
    {
        //Debug.Log(gamepad.ToString());
        //Debug.Log(gamepad.name);
        //Debug.Log(gamepad.description);

        if (gamepad.name.Equals("DualShock4GamepadHID"))
        {
            //Debug.Log("ps4");
            return 1;
        }
        if (gamepad.name.Equals("XInputControllerWindows"))
        {
            //Debug.Log("third party xbox");
            return 0.4f;
        }

        //TODO: more generic solution? currently modeled mainly for ps4 controller

        return 1;
    }

    public static Color turnTeamColorDark(Color orgColor, float brigthness)
    {
        var darkColor = orgColor;
        float H;
        float S;
        float V;
        Color.RGBToHSV(darkColor, out H, out S, out V);
        V = brigthness;
        darkColor = Color.HSVToRGB(H, S, V);
        //print(darkColor);
        return darkColor;
    }
}

