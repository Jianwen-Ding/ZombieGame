using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcProgram : MonoBehaviour
{
    static public float getDist3D(float x, float y, float z)
    {
        return Mathf.Sqrt(x * x + y * y + z + z);
    }
    static public float getAngle2D(float x, float y)
    {
        return ;
    }
}
