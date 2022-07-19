using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isInCollision : MonoBehaviour
{
    [SerializeField]
    bool isColliding = false;
    [SerializeField]
    bool isGrounded = false;
    [SerializeField]
    float rectExpand;
    public bool getIsColliding()
    {
        return isColliding;
    }
    public bool getIsGrounded()
    {
        return isGrounded;
    }
    private void OnTriggerEnter(Collider other)
    {
        isColliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
    private void Update()
    {
        int layerMask = ~LayerMask.GetMask("Player");
        isGrounded = (isColliding && (Physics.Raycast(gameObject.transform.position, new Vector3(0, -1, 0), 2, layerMask)
            || Physics.Raycast(new Vector3(transform.position.x + rectExpand, transform.position.y, transform.position.z+ rectExpand), new Vector3(0, -1, 0), 0.85f, layerMask) 
            || Physics.Raycast(new Vector3(transform.position.x - rectExpand, transform.position.y, transform.position.z + rectExpand), new Vector3(0, -1, 0), 0.85f, layerMask) 
            || Physics.Raycast(new Vector3(transform.position.x + rectExpand, transform.position.y, transform.position.z - rectExpand), new Vector3(0, -1, 0), 0.85f, layerMask) 
            || Physics.Raycast(new Vector3(transform.position.x - rectExpand, transform.position.y, transform.position.z - rectExpand), new Vector3(0, -1, 0), 0.85f, layerMask)));
        /*Debug.DrawRay(gameObject.transform.position, new Vector3(0, -0.85f, 0), Color.red, 0.5f);
        Debug.DrawRay(new Vector3(transform.position.x - rectExpand, transform.position.y, transform.position.z + rectExpand), new Vector3(0, -0.85f, 0), Color.red, 0.5f);
        Debug.DrawRay(new Vector3(transform.position.x + rectExpand, transform.position.y, transform.position.z + rectExpand), new Vector3(0, -0.85f, 0), Color.red, 0.5f);
        Debug.DrawRay(new Vector3(transform.position.x - rectExpand, transform.position.y, transform.position.z - rectExpand), new Vector3(0, -0.85f, 0), Color.red, 0.5f);
        Debug.DrawRay(new Vector3(transform.position.x + rectExpand, transform.position.y, transform.position.z - rectExpand), new Vector3(0, -0.85f, 0), Color.red, 0.5f);*/
    }
}
