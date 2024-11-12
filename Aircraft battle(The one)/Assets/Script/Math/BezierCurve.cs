using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve 
{
    public static Vector3 QuadraticPoint(Vector3 start, Vector3 end,Vector3 controlPoint,float by)
    {
        return Vector3.Lerp(Vector3.Lerp(start, controlPoint, by),
            Vector3.Lerp(controlPoint, end, by), by);               
    }
}
