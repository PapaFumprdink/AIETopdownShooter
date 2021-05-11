using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public static class Util
{
    public static int Loop (int val, int max)
    {
        if (val < 0)
        {
            return max + ((val + 1) % max) - 1;
        }
        else
        {
            return val % max;
        }
    }

    public static Vector2 VectorFromAngle(float angle) => new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

    public static float AngleDifference (float angleA, float angleB)
    {
        float optionA = angleA - angleB;
        float optionB = angleB - angleA;

        return Mathf.Abs(optionA) < Mathf.Abs(optionB) ? optionA : optionB;
    }
}
