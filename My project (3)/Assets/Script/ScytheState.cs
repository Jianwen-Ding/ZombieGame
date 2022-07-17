using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheState : MonoBehaviour
{
    //establish
    [SerializeField]
    BoxCollider collide;
    GameObject grabbedBy;
    Rigidbody rb;
   
    
    //States
    //"grabbed"
    //"flying"
    //"stuck"
    [SerializeField]
    string scytheState;
    //grabbed scytheState
    float timeCharge;
    bool hasCharged;
    //flying scytheState
    //
    Quaternion rotFly;
    Vector3 posFly;
    [SerializeField]
    float zAxisRotateOnRelease;
    [SerializeField]
    float xAxisChange;
    [SerializeField]
    float speedToSpinRatio;
    [SerializeField]
    float rotationDebug;
    //stuck scytheState
    GameObject stabbedObject;
    public void establishGrabbedBy(GameObject establish)
    {
        grabbedBy = establish;
    }
    public void establishGrab()
    {
        scytheState = "grabbed";
    }
    public void endGrab(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        scytheState = "flying";
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(scytheState == "flying")
        {
        }
        
    }
}
