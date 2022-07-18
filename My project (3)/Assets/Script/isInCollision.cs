using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isInCollision : MonoBehaviour
{
    bool isColliding = false;
    public bool getIsColliding()
    {
        return isColliding;
    }
    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
    }
    private void OnCollision(Collision collision)
    {
        isColliding = true;
    }
}
