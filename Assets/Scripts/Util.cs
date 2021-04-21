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
}
