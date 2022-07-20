using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheGrab : MonoBehaviour
{
    GameObject player;
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
    #endregion
    #region pull
    [SerializeField]
    Vector2 anglesScytheCurrent;
    [SerializeField]
    float angleSpeedChange;
    [SerializeField]
    Vector2 angleOffSetFromController;
    [SerializeField]
    float angleDiffrenceChange;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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


    // Update is called once per frame
    void Update()
    {
        scytheCurrentlyInRange = Vector3.Distance(player.transform.position, this.transform.position) < 4;
        designatedState.setSlash(slashState == "slash");
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
            if(slashState != "uncharged")
            {
                slashState = "uncharged";
                designatedScythe.GetComponent<MeshRenderer>().material = unchargedMat;
            }
           
            if (rightHand && OVRInput.Get(OVRInput.Button.SecondaryThumbstick) || rightHand == false && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
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
            if (rightHand && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) != 0 || rightHand == false && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) != 0)
            {
                Vector2 targetAngle;
                float distFromAnch;
                distFromAnch = CalcProgram.getDistBetweenPoints3D(designatedScythe.transform.position, contTransform.position);
                anglesScytheCurrent = CalcProgram.getAngleBetweenPoints3D(designatedScythe.transform.position, contTransform.position);
                targetAngle = new Vector2(camRig.rightControllerAnchor.rotation.x, contTransform.rotation.y);
                //if(angleDiffrenceChange > Mathf.Abs(targetAngle.x - anglesScytheCurrent.x) && angleDiffrenceChange > Mathf.Abs(targetAngle.y - anglesScytheCurrent.y))
                //{
                Vector2 angleChange = CalcProgram.getVectorFromAngle2D(CalcProgram.getAngleBetweenPoints2D(targetAngle, anglesScytheCurrent), angleSpeedChange * Time.deltaTime);
                anglesScytheCurrent = new Vector2(anglesScytheCurrent.x + angleChange.x, anglesScytheCurrent.y + angleChange.y);
                designatedScythe.transform.position = CalcProgram.getVectorFromAngle3D(anglesScytheCurrent.x, anglesScytheCurrent.y, distFromAnch);
                Vector3 lockedVel = designatedState.getLockedVel();
                float velSpeed = CalcProgram.getDist3D(lockedVel);
                designatedState.setLockedVel(CalcProgram.getVectorFromAngle3D(angleChange.x, angleChange.y, velSpeed));
                //}
                
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
                        designatedScythe.GetComponent<MeshRenderer>().material = chargedMat;
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
                designatedScythe.GetComponent<MeshRenderer>().material = chargedMat;
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
                    designatedScythe.GetComponent<MeshRenderer>().material = slashMat;
                    timeSlashLeft = timeSlash;
                }
            }
            if (slashState == "slash")
            {
                designatedScythe.GetComponent<MeshRenderer>().material = slashMat;
                timeSlashLeft -= Time.deltaTime;
                if(timeSlashLeft <= 0)
                {
                    slashState = "uncharged";
                    designatedScythe.GetComponent<MeshRenderer>().material = unchargedMat;
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
                    designatedScythe.GetComponent<MeshRenderer>().material = unchargedMat;
                }
            }
        }
    }
}
