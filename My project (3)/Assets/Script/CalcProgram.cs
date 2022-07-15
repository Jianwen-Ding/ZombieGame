using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcProgram : MonoBehaviour
{
    //uses degrees
    static public float getDist2D(float x, float y)
    {
        return Mathf.Sqrt(x * x + y * y);
    }
    static public float getDist3D(float x, float y, float z)
    {
        return Mathf.Sqrt(x * x + y * y + z + z);
    }
    static public Vector2 getVectorFromAngle2D(float angle, float distance)
    {
        Vector2 ret;
        ret.x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        ret.y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;
        return ret;
    }
    static public Vector3 getVectorFromAngle3D(float angleX, float angleY, float distance)
    {
        Vector3 ret;
        ret.x = getVectorFromAngle2D(angleX, distance).x;
        ret.y = getVectorFromAngle2D(angleX, distance).y;
        ret.z = Mathf.Sin(angleY * Mathf.Deg2Rad) * distance;
        return ret;
    }
    //Output is in degree
    static public float getAngle2D(float x, float y)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(y, x);
    }
    static public float getAngleBetweenPoints2D(float x, float y, float x2, float y2)
    {
        float xDif = x2 - x;
        float yDif = y2 - y;
        return getAngle2D(xDif, yDif);
        
    }
    //angleX
    //angleY
    static public Vector2 getAngleDiffrence3D(float x, float y, float z, float x2, float y2, float z2)
    {
        Vector2 ret;
        ret.x = Mathf.Rad2Deg * Mathf.Atan2(z, x);
        ret.y = Mathf.Rad2Deg * Mathf.Atan2(y, z);
        return ret;
    }
}