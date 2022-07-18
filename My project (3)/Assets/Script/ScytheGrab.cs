using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheGrab : MonoBehaviour
{
    
    #region debug
    [SerializeField]
    bool debugBool;
    [SerializeField]
    bool debugRelease;
    bool hasTapped;
    #endregion
    #region establish
    [SerializeField]
    LineRenderer line;
    [SerializeField]
    bool rightHand;
    [SerializeField]
    Transform contTransform;
    [SerializeField]
    GameObject designatedScythe;
    ScytheState designatedState;
    [SerializeField]
    OVRCameraRig camRig;
    [SerializeField]
    GameObject mainController;
    [SerializeField]
    isInCollision checkCol;
    #endregion
    #region 
    [SerializeField]
    Vector3 scytheOffset;
    [SerializeField]
    Vector3 scytheAngle;
    #endregion
    #region grab var
    [SerializeField]
    bool scytheCurrentlyInRange;
    [SerializeField]
    bool scytheGrabbed;
    #endregion
    #region release var
    [SerializeField]
    Vector3 anchorOffsetPosition;
    [SerializeField]
    Quaternion anchorOffsetRotation;
    #endregion
    #region line
    [SerializeField]
    float lineUpAdjust;
    #endregion
    #region slash
    [SerializeField]
    Material unchargedMat;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 2;
        if (designatedScythe != null)
        {
            designatedState = designatedScythe.GetComponent<ScytheState>();
            designatedState.establishGrabbedBy(gameObject);
        }
       
    }
    protected virtual void Awake()
    {
        OVRCameraRig[] CameraRigs = gameObject.GetComponentsInParent<OVRCameraRig>();

        if (CameraRigs.Length == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (CameraRigs.Length > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            camRig = CameraRigs[0];
        checkCol = GameObject.FindGameObjectWithTag("PlayerCollider").GetComponent<isInCollision>();
        anchorOffsetPosition = contTransform.localPosition;
        anchorOffsetRotation = contTransform.localRotation;
        camRig.UpdatedAnchors += (r) => { OnUpdatedAnchors(); };
    }
    void OnUpdatedAnchors()
    {
        if (scytheGrabbed)
        {
            designatedScythe.transform.position = scytheOffset + contTransform.position;
            designatedScythe.transform.rotation = contTransform.rotation;
            designatedScythe.transform.Rotate(scytheAngle);
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
            /*line.SetPosition(0, new Vector3(contTransform.position.x, contTransform.position.y + lineUpAdjust, contTransform.position.z));
            line.SetPosition(1, designatedScythe.transform.position);*/

            if ((rightHand && OVRInput.Get(OVRInput.Button.SecondaryThumbstick) || rightHand == false && OVRInput.Get(OVRInput.Button.PrimaryThumbstick)))
            {
                designatedScythe.transform.parent = null;
                mainController.transform.position = designatedScythe.transform.position;
                scytheGrabbed = true;
                if (designatedScythe.GetComponent<ScytheState>() != null)
                {
                    designatedState.establishGrab();
                }
            }
            if (scytheCurrentlyInRange)
            {
                if (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
                {
                    if (hasTapped == false)
                    {
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
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
            if (rightHand && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.One) || rightHand == false && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
            {
                if (hasTapped == false)
                {
                    OVRPose localPose;
                    OVRPose offsetPose;
                    Vector3 linearVelocity;
                    Vector3 angularVelocity;

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
                    scytheGrabbed = false;
                }
                hasTapped = true;
            }
            else
            {
                hasTapped = false;
            }
            OnUpdatedAnchors();
        }
    }
}
