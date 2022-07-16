using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheGrab : MonoBehaviour
{
    
    [SerializeField]
    bool debugBool;
    bool hasTapped;
    //establish
    [SerializeField]
    bool rightHand;
    [SerializeField]
    Transform contTransform;
    //Offset
    [SerializeField]
    Vector3 scytheOffset;
    [SerializeField]
    Vector3 scytheAngle;
    [SerializeField]
    GameObject designatedScythe;
    //grab var
    Vector3 lastPos;
    Quaternion lastRot;
    bool scytheCurrentlyInRange;
    bool scytheGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;
    }
    void OnUpdatedAnchors()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;
    }
        void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.gameObject == designatedScythe)
        {
            scytheCurrentlyInRange = true;
        }
        
    }
    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.gameObject == designatedScythe)
        {
            scytheCurrentlyInRange = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (debugBool)
        {
            designatedScythe.transform.position = contTransform.position;
            Rigidbody grabbedRigidbody = gameObject.GetComponent<Rigidbody>();
            Vector3 grabbablePosition = lastPos + lastRot * contTransform.localPosition;
            Quaternion grabbableRotation = lastRot * contTransform.localRotation;
            grabbedRigidbody.transform.position = grabbablePosition;
            grabbedRigidbody.transform.rotation = grabbableRotation;
            designatedScythe.transform.parent = gameObject.transform;
            designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
            designatedScythe.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            scytheGrabbed = true;
        }
        if (scytheGrabbed == false)
        {
            if (scytheCurrentlyInRange)
            {
                if (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
                {
                    if (hasTapped == false)
                    {
                        designatedScythe.transform.position = contTransform.position;
                        Rigidbody grabbedRigidbody = gameObject.GetComponent<Rigidbody>();
                        Vector3 grabbablePosition = lastPos + lastRot * contTransform.localPosition;
                        Quaternion grabbableRotation = lastRot * contTransform.localRotation;
                        grabbedRigidbody.transform.position = grabbablePosition;
                        grabbedRigidbody.transform.rotation = grabbableRotation;
                        designatedScythe.transform.parent = gameObject.transform;
                        designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        designatedScythe.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        scytheGrabbed = true;
                        designatedScythe.GetComponent<ScytheState>().establishGrab();
                    }
                    hasTapped = true;
                }
                else
                {
                    hasTapped = false;
                }
            }
        }
        else
        {
            designatedScythe.transform.position = contTransform.position;
            Rigidbody grabbedRigidbody = gameObject.GetComponent<Rigidbody>();
            Vector3 grabbablePosition = lastPos + lastRot * contTransform.localPosition;
            Quaternion grabbableRotation = lastRot * contTransform.localRotation;
            grabbedRigidbody.transform.position = grabbablePosition;
            grabbedRigidbody.transform.rotation = grabbableRotation;
            designatedScythe.transform.parent = gameObject.transform;
            designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
            designatedScythe.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
                {
                if (hasTapped == false)
                {
                    designatedScythe.GetComponent<ScytheState>().endGrab();
                    designatedScythe.transform.parent = null;
                    scytheGrabbed = false;
                }
                hasTapped = true;
            }
            else
            {
                hasTapped = false;
            }
        }
        if (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
        {
            hasTapped = true;
        }
        else
        {
            hasTapped = false;
        }
    }
}
