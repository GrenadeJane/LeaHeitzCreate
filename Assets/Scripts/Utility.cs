using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static float ClampAngle(float angle)
    {
        angle = angle % (2 * Mathf.PI);
        if (angle < 0)
        {
            angle += (2 * Mathf.PI);
        }
        return angle;
    }
}
