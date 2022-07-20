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
    static public float getDist2D(Vector2 vector)
    {
        return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
    }
    static public float getDist3D(float x, float y, float z)
    {
        return Mathf.Sqrt(x * x + y * y + z * z);
    }
    static public float getDist3D(Vector3 point)
    {
        return Mathf.Sqrt(point.x * point.x + point.y * point.y + point.z * point.z);
    }
    static public float getDistBetweenPoints2D(float x, float y, float x1, float y1)
    {
        float xDif = x - x1;
        float yDif = y - y1;
        return Mathf.Sqrt(xDif * xDif + yDif * yDif);
    }
    static public float getDistBetweenPoints2D(Vector2 vector, Vector2 origin)
    {
        float xDif = vector.x - origin.x;
        float yDif = vector.y - origin.y;
        return Mathf.Sqrt(xDif * xDif + yDif * yDif);
    }
    static public float getDistBetweenPoints3D(float x, float y, float z, float x1, float y1, float z1)
    {
        float xDif = x - x1 ;
        float yDif = y - y1;
        float zDif = z - z1;
        return Mathf.Sqrt(xDif * xDif + yDif * yDif + zDif + zDif);
    }
    static public float getDistBetweenPoints3D(Vector3 vector, Vector3 origin)
    {
        float xDif = vector.x - origin.x;
        float yDif = vector.y - origin.y;
        float zDif = vector.z - origin.z;
        return Mathf.Sqrt(xDif * xDif + yDif * yDif + zDif + zDif);
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
        float xUse = angleX % 360;
        float yUse = angleY % 360;
        Vector3 ret;
        ret.y = Mathf.Sin(yUse * Mathf.Deg2Rad) * distance;
        float diameterXAxis = Mathf.Cos(yUse * Mathf.Deg2Rad) * distance;
        ret.x = getVectorFromAngle2D(xUse, diameterXAxis).x;
        ret.z = getVectorFromAngle2D(xUse, diameterXAxis).y;
        return ret;
    }
    static public Vector2 getVectorFromAngleBetweenTwoPoints2D(float angle, Vector2 origin, float distance)
    {
        Vector2 ret;
        ret.x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        ret.y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;
        ret = new Vector2(ret.x + origin.x , ret.y + origin.y);
        return ret;
    }
    static public Vector3 getVectorFromAngleBetweenTwoPoints3D(float angleX, float angleY, Vector3 origin, float distance)
    {
        Vector3 ret = getVectorFromAngle3D(angleX, angleY, distance);
        ret = new Vector3(ret.x + origin.x, ret.y + origin.y, ret.z + origin.z);
        return ret;
    }
    static public Vector2 DegreesFromVector(Vector3 angle)
    {
        Vector2 ret;
        ret.x = getAngle2D(angle.x, angle.z);
        ret.y = getAngle2D(angle.z, angle.y);
        return ret;
    }
    //Output is in degree
    static public float getAngle2D(float x, float y)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(y, x);
    }
    static public float getAngle2D(Vector2 vector)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x);
    }
    static public float getAngleBetweenPoints2D(float x, float y, float x2, float y2)
    {
        float xDif = x2 - x;
        float yDif = y2 - y;
        return getAngle2D(xDif, yDif);
        
    }
    static public float getAngleBetweenPoints2D(Vector2 vector, Vector2 origin)
    {
        float xDif = vector.x - origin.x;
        float yDif = vector.y - origin.y;
        return getAngle2D(xDif, yDif);

    }
    //angleX
    //angleY
    static public Vector2 getAngle3D(float x, float y, float z)
    {
        bool xPos = x >= 0;
        bool zPos = z >= 0;
        Vector2 ret;
        ret.x = Mathf.Rad2Deg * Mathf.Atan2(z, x);
        float dist = getDist2D(x,z);
        ret.y = Mathf.Rad2Deg * Mathf.Atan2(y, dist);
        ret.x %= 360;
        ret.y %= 360;
        if (ret.x < 0)
        {
            ret.x = 360 + ret.x;
        }
        if (ret.y < 0)
        {
            ret.y = 360 + ret.y ;
        }
        if (xPos == true && zPos == true)
        {
            ret.x = (ret.x % 90);
        }
        if (xPos ==false && zPos == true)
        {
            ret.x = (ret.x % 90) + 90;
        }
        if (xPos == false && zPos == false)
        {
            ret.x = (ret.x % 90) + 180;
        }
        if (xPos == true && zPos == false)
        {
            ret.x = (ret.x % 90) + 270;
        }
       
        return ret;
    }
    static public Vector2 getAngle3D(Vector3 vector)
    {
        return getAngle3D(vector.x, vector.y, vector.z);
    }
    static public Vector2 getAngleBetweenPoints3D(float x, float y, float z, float x1, float y1, float z1)
    {
        Vector2 ret = getAngle3D(new Vector3(x - x1, y - y1, z - z1));
        return ret;
    }
    static public Vector2 getAngleBetweenPoints3D(Vector3 vector, Vector3 origin)
    {
        Vector2 ret = getAngle3D(new Vector3(vector.x - origin.x, vector.y - origin.y, vector.z - origin.z));
        return ret;
    }
}