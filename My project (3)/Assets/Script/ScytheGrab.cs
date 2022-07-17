using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheGrab : MonoBehaviour
{
    
    //Debug
    [SerializeField]
    bool debugBool;
    [SerializeField]
    bool debugRelease;
    bool hasTapped;
    //establish
    [SerializeField]
    bool rightHand;
    [SerializeField]
    Transform contTransform;
    [SerializeField]
    GameObject designatedScythe;
    ScytheState designatedState;
    //Offset
    [SerializeField]
    Vector3 scytheOffset;
    [SerializeField]
    Vector3 scytheAngle;
    //grab var
    Vector3 lastPos;
    Quaternion lastRot;
    bool scytheCurrentlyInRange;
    bool scytheGrabbed;
    //release var
    Vector3 scythePos;
    Quaternion scytheRot;
    Vector3 anchorOffsetPosition;
    Quaternion anchorOffsetRotation;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;
        if(designatedScythe != null)
        {
            scythePos = designatedScythe.transform.localPosition;
            scytheRot = designatedScythe.transform.localRotation;
            designatedState = designatedScythe.GetComponent<ScytheState>();
            designatedState.establishGrabbedBy(gameObject);
        }
       
    }
    void OnUpdatedAnchors()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;
        if (scytheGrabbed)
        {
            scythePos = designatedScythe.transform.position;
            scytheRot = designatedScythe.transform.rotation;
        }
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
            Quaternion angleOffSet = Quaternion.Euler(scytheAngle);
            designatedScythe.transform.position = scytheOffset + contTransform.position;
            Rigidbody grabbedRigidbody = gameObject.GetComponent<Rigidbody>();
            Vector3 grabbablePosition = lastPos + lastRot * contTransform.localPosition;
            Quaternion grabbableRotation = lastRot * (angleOffSet * contTransform.localRotation);
            grabbedRigidbody.transform.position = grabbablePosition;
            grabbedRigidbody.transform.rotation = grabbableRotation;
            designatedScythe.transform.parent = gameObject.transform;
            designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
            designatedScythe.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            scytheGrabbed = true;
            debugBool = false;
        }
        if (debugRelease)
        {
            designatedScythe.transform.parent = null;
            scytheGrabbed = false;
            debugRelease = false;
        }
        if (scytheGrabbed == false)
        {
            if (scytheCurrentlyInRange)
            {
                if (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
                {
                    if (hasTapped == false)
                    {
                        Quaternion angleOffSet = Quaternion.Euler(scytheAngle);
                        designatedScythe.transform.position =  scytheOffset + contTransform.position;
                        Rigidbody grabbedRigidbody = gameObject.GetComponent<Rigidbody>();
                        Vector3 grabbablePosition = lastPos + lastRot * contTransform.localPosition;
                        Quaternion grabbableRotation = lastRot * (angleOffSet * contTransform.localRotation);
                        grabbedRigidbody.transform.position = grabbablePosition;
                        grabbedRigidbody.transform.rotation = grabbableRotation;
                        designatedScythe.transform.parent = gameObject.transform;
                        designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        designatedScythe.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        scytheGrabbed = true;
                        if(designatedScythe.GetComponent<ScytheState>() != null)
                        {
                            designatedScythe.GetComponent<ScytheState>().establishGrab();
                        }
                        
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
            bool hasReleased = false;
            if (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
            {
                if (hasTapped == false)
                {
                    OVRPose localPose;
                    OVRPose offsetPose;
                    Vector3 linearVelocity;
                    Vector3 angularVelocity;
                    designatedScythe.transform.localPosition = scytheOffset + contTransform.transform.localPosition;
                    designatedScythe.transform.localRotation = contTransform.transform.localRotation;
                    if (rightHand)
                    {
                        localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) };
                        offsetPose = new OVRPose { position = anchorOffsetPosition, orientation = anchorOffsetRotation };
                        localPose = localPose * offsetPose;
                        OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                        linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                        angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
                    }
                    else
                    {
                        localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) };
                        offsetPose = new OVRPose { position = anchorOffsetPosition, orientation = anchorOffsetRotation };
                        localPose = localPose * offsetPose;
                        OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                        linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                        angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
                    }
                    if (designatedScythe.GetComponent<ScytheState>() != null)
                    {
                        designatedScythe.GetComponent<ScytheState>().endGrab(linearVelocity, angularVelocity);
                    }
                    designatedScythe.transform.parent = null;
                    hasReleased = true;
                    scytheGrabbed = false;
                    /*Quaternion angleOffSet = Quaternion.Euler(scytheAngle);
                    designatedScythe.transform.localPosition = scytheOffset + designatedScythe.transform.localPosition;
                    designatedScythe.transform.localRotation = designatedScythe.transform.localRotation;
                    designatedScythe.transform.Rotate(scytheAngle);
                    Rigidbody grabbedRigidbody = designatedScythe.GetComponent<Rigidbody>();
                    Vector3 grabbablePosition = scythePos + scytheRot * designatedScythe.transform.localPosition;
                    Quaternion grabbableRotation = scytheRot * (angleOffSet * designatedScythe.transform.localRotation);
                    grabbedRigidbody.transform.position = grabbablePosition;
                    grabbedRigidbody.transform.rotation = grabbableRotation;
                    grabbedRigidbody.velocity = Vector3.zero;
                    grabbedRigidbody.angularVelocity = Vector3.zero;
                    */
                }
                hasTapped = true;
            }
            else
            {
                hasTapped = false;
            }
            if (hasReleased == false)
            {
                Quaternion angleOffSet = Quaternion.Euler(scytheAngle);
                designatedScythe.transform.localPosition = scytheOffset + contTransform.localPosition;
                designatedScythe.transform.localRotation = contTransform.localRotation;
                designatedScythe.transform.Rotate(scytheAngle);
                Rigidbody grabbedRigidbody = gameObject.GetComponent<Rigidbody>();
                Vector3 grabbablePosition = lastPos + lastRot * contTransform.localPosition;
                Quaternion grabbableRotation = lastRot * (angleOffSet * contTransform.localRotation);
                grabbedRigidbody.transform.position = grabbablePosition;
                grabbedRigidbody.transform.rotation = grabbableRotation;
                designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
                designatedScythe.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }
}
