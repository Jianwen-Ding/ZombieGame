using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheState : MonoBehaviour
{
    //establish
    [SerializeField]
    BoxCollider collide;
    OVRGrabbable grab;
    Rigidbody rb;
    bool isRight;
    
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
    // Start is called before the first frame update
    void Start()
    {
        //establish
        rb = gameObject.GetComponent<Rigidbody>();
        grab = gameObject.GetComponent<OVRGrabbable>();
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if(scytheState == "flying" && other.gameObject.layer != 7)
        {
            gameObject.transform.parent = other.gameObject.transform;
            scytheState = "stuck";
            rb.velocity = Vector3.zero;
        }*/
        
    }
    // Update is called once per frame
    void Update()
    {
       if(grab.grabbedBy != null)
        {
            scytheState = "grabbed";
            gameObject.transform.parent = null;
        }
       if(scytheState == "grabbed")
        {
            rb.useGravity = true;
            if(grab.grabbedBy == null)
            {
                scytheState = "flying";
            }
        }
       if(scytheState == "flying")
        {
            rb.useGravity = false;
            float speed = Mathf.Abs(CalcProgram.getDist3D(rb.velocity.x , rb.velocity.y, rb.velocity.z));
            xAxisChange += speed * speedToSpinRatio * Time.deltaTime;
            transform.rotation = Quaternion.Euler(xAxisChange, transform.rotation.y, zAxisRotateOnRelease);
            
            
        }
        if (scytheState == "stuck")
        {
            rb.velocity = Vector3.zero;
        }
    }
}
