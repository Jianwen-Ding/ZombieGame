using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheGrab : MonoBehaviour
{
    [SerializeField]
    bool debugBool;
    bool hasTapped;
    [SerializeField]
    bool rightHand;
    [SerializeField]
    Transform contTransform;
    [SerializeField]
    Vector3 scytheOffset;
    [SerializeField]
    Vector3 scytheAngle;
    float zAngleOffSet;
    [SerializeField]
    GameObject designatedScythe;
    bool scytheCurrentlyInRange;
    bool scytheGrabbed;
    // Start is called before the first frame update
    void Start()
    {
        
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
            designatedScythe.transform.position = new Vector3(contTransform.position.x + scytheOffset.x, contTransform.position.y + scytheOffset.y, contTransform.position.z + scytheOffset.z);
            designatedScythe.transform.localRotation = Quaternion.Euler(contTransform.localRotation.x + scytheAngle.x, contTransform.localRotation.y + scytheAngle.y, contTransform.localRotation.z + scytheAngle.z);
            designatedScythe.transform.parent = gameObject.transform;
            designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
            designatedScythe.GetComponent<Rigidbody>().freezeRotation = true;
            designatedScythe.GetComponent<Rigidbody>().useGravity = false;
            scytheGrabbed = true;
        }
        if (scytheGrabbed == false)
        {
            if (scytheCurrentlyInRange)
            {
                if (hasTapped == false && (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four)))
                {
                    designatedScythe.transform.position = new Vector3(contTransform.position.x + scytheOffset.x, contTransform.position.y + scytheOffset.y, contTransform.position.z + scytheOffset.z);
                    designatedScythe.transform.rotation = Quaternion.Euler(contTransform.rotation.x + scytheAngle.x, contTransform.rotation.y + scytheAngle.y, contTransform.rotation.z + scytheAngle.z);
                    designatedScythe.transform.parent = gameObject.transform;
                    designatedScythe.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    designatedScythe.GetComponent<Rigidbody>().freezeRotation = true;
                    designatedScythe.GetComponent<Rigidbody>().useGravity = false;
                    scytheGrabbed = true;
                }
            }
        }
        else
        {
            if (scytheGrabbed == false)
            {
                return;
            }

            Rigidbody grabbedRigidbody = designatedScythe.GetComponent<Rigidbody>();
            Quaternion grabbableRotation = Quaternion.Euler(scytheAngle) * contTransform.localRotation;
            grabbedRigidbody.transform.rotation = grabbableRotation;
            if (hasTapped == false && (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four)))
            {
                /*OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
                OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
                localPose = localPose * offsetPose;

                OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
                Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);*/
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
