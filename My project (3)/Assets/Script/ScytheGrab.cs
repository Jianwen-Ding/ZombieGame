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
    //states include
    //"uncharged"
    //"charged"
    //"slash"
    [SerializeField]
    BoxCollider slashCollider;
    [SerializeField]
    string slashState;
    [SerializeField]
    Material unchargedMat;
    [SerializeField]
    Material chargedMat;
    [SerializeField]
    Material slashMat;
    //how far scythe is from camera angle until it charges
    [SerializeField]
    float xAxisThreshold;
    //Time it takes for scythe to charge left(don't set this)
    [SerializeField]
    float timeChargeLeft;
    //Time it takes for scythe to charge (set this)
    [SerializeField]
    float timeCharge;
    //Slash timers
    [SerializeField]
    float timeSlashLeft;
    [SerializeField]
    float timeSlash;
    //Speed attack threshold
    [SerializeField]
    float velocitySlashThreshold;
    //Speed attack threshold
    [SerializeField]
    float velocitySlashMinimum;
    //Damage it deals
    [SerializeField]
    int damageSlashDeal;
    //Amount slashed rigidbodies get slashed back
    [SerializeField]
    int slashPushBack;
    #endregion
    #region pull
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
        if(slashState == "slash")
        {
            if(otherCollider.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector2 angle = CalcProgram.getAngleBetweenPoints3D(otherCollider.gameObject.transform.position, camRig.centerEyeAnchor.position);
                otherCollider.gameObject.GetComponent<Rigidbody>().AddForce(CalcProgram.getVectorFromAngle3D(angle.x, angle.y, slashPushBack));
            }
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
        if (slashState != "slash")
        {
            slashCollider.enabled = false;
        }
            if (debugBool)
        {
            scytheGrabbed = true;
            debugBool = false;
        }
        if (debugRelease)
        {
            designatedScythe.transform.parent.parent = null;
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
            //Slash controls
            float cameraAngle = PlayerMainScript.solePlayer.GetComponent<PlayerMainScript>().findCamAngle();
            float angleOfSythe = CalcProgram.getAngle2D(designatedScythe.transform.position.x - camRig.centerEyeAnchor.position.x, designatedScythe.transform.position.z - camRig.centerEyeAnchor.position.z);
            if(slashState == "uncharged")
            {
                designatedScythe.GetComponent<MeshRenderer>().material = unchargedMat;
                if (Mathf.Abs(angleOfSythe - cameraAngle) > xAxisThreshold)
                {
                    timeChargeLeft -= Time.deltaTime;
                    if (timeChargeLeft <= 0)
                    {
                        slashState = "charged";
                        timeChargeLeft = timeCharge;
                    }
                }
                else
                {
                    timeChargeLeft = timeCharge;
                }
            }
            if (slashState == "charged")
            {
                designatedScythe.GetComponent<MeshRenderer>().material = slashMat;
                OVRPose localPose;
                OVRPose offsetPose;
                Vector3 linearVelocity;
                if (rightHand)
                {
                    localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) };
                    offsetPose = new OVRPose { position = anchorOffsetPosition, orientation = anchorOffsetRotation };
                    localPose = localPose * offsetPose;
                    OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                    linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);                
                }
                else
                {
                    localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) };
                    offsetPose = new OVRPose { position = anchorOffsetPosition, orientation = anchorOffsetRotation };
                    localPose = localPose * offsetPose;
                    OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                    linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);                
                }
                if (CalcProgram.getDist3D(linearVelocity) > velocitySlashThreshold)
                {
                    slashState = "slash";
                    timeSlashLeft = timeSlash;
                }
            }
            if (slashState == "slash")
            {
                designatedScythe.GetComponent<MeshRenderer>().material = chargedMat;
                slashCollider.enabled = true;
                timeSlashLeft -= Time.deltaTime;
                if(timeSlashLeft <= 0)
                {
                    slashState = "uncharged";
                }
                OVRPose localPose;
                OVRPose offsetPose;
                Vector3 linearVelocity;
                if (rightHand)
                {
                    localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) };
                    offsetPose = new OVRPose { position = anchorOffsetPosition, orientation = anchorOffsetRotation };
                    localPose = localPose * offsetPose;
                    OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                    linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                }
                else
                {
                    localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) };
                    offsetPose = new OVRPose { position = anchorOffsetPosition, orientation = anchorOffsetRotation };
                    localPose = localPose * offsetPose;
                    OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                    linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                }
                if (CalcProgram.getDist3D(linearVelocity) < velocitySlashMinimum)
                {
                    slashState = "uncharged";
                }
            }
        }
    }
}
