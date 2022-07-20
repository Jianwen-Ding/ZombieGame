using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheState : MonoBehaviour
{
    //establish
    [SerializeField]
    BoxCollider collide;
    [SerializeField]
    GameObject grabbedBy;
    Rigidbody rb;
    [SerializeField]
    OVRCameraRig camRig;
    [SerializeField]
    Vector3 setSize;
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
    bool xChangePositive;
    [SerializeField]
    Vector3 lockedVelocity;
    [SerializeField]
    float zAxisRotateOnRelease;
    [SerializeField]
    float yAxisRotateOnRelease;
    [SerializeField]
    float xAxisChange;
    [SerializeField]
    float speedToSpinRatio;
    [SerializeField]
    float rotationDebug;
    [SerializeField]
    float damageThrowDeal;
    [SerializeField]
    float minimumThrowSpeed = 3f;
    [SerializeField]
    float throwSpeedMultiplier = 2f;
    //stuck scytheState
    GameObject stabbedObject;
    [SerializeField]
    float hitImpulse;
    //slash
    [SerializeField]
    bool isSlashing;
    [SerializeField]
    float knockBackSlash;
    [SerializeField]
    int damageOnSlash;
    //Size of hitbox
    [SerializeField]
    Vector3 slashSizeIncrease;
    [SerializeField]
    Vector3 slashOriginalSize;
    public void setLockedVel(Vector3 lockSet)
    {
        lockedVelocity = lockSet;
    }
    public Vector3 getLockedVel()
    {
        return lockedVelocity;
    } 
    public void setSlash(bool set)
    {
        isSlashing = set;
    }
    public bool getSlash()
    {
        return isSlashing;
    }
    public void establishGrabbedBy(GameObject establish)
    {
        grabbedBy = establish;
    }
    public void establishGrab()
    {
        rb.isKinematic = true;
        scytheState = "grabbed";
        gameObject.transform.localScale = setSize;
    }
    public void endGrab(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        scytheState = "flying";
        float speed = CalcProgram.getDist3D(linearVelocity);
        Vector2 angles = CalcProgram.getAngle3D(linearVelocity);
        speed *= throwSpeedMultiplier;
        if(speed < minimumThrowSpeed)
        {
            lockedVelocity = CalcProgram.getVectorFromAngle3D(angles.x , angles.y, minimumThrowSpeed);
        }
        else
        {
            lockedVelocity = CalcProgram.getVectorFromAngle3D(angles.x, angles.y, speed);
        }
        rb.velocity = lockedVelocity;
        if(angularVelocity.x > 0)
        {
            xChangePositive = true;
        }
        else
        {
            xChangePositive = false;
        }
        rb.angularVelocity = Vector3.zero;
        zAxisRotateOnRelease = gameObject.transform.rotation.z;
        xAxisChange = gameObject.transform.rotation.x;
        yAxisRotateOnRelease = gameObject.transform.rotation.y;
    }
    // Start is called before the first frame update
    void Start()
    {
        collide = gameObject.GetComponent<BoxCollider>();
        slashOriginalSize = collide.size;
        setSize = gameObject.transform.localScale;
        OVRCameraRig[] CameraRigs = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<OVRCameraRig>();

        if (CameraRigs.Length == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (CameraRigs.Length > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            camRig = CameraRigs[0];
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Hands" && scytheState == "flying" &&  other.gameObject.layer != 6)
        {
            stabbedObject = other.gameObject;
            scytheState = "stuck";
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            if (other.gameObject.GetComponent<Rigidbody>() != null)
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(lockedVelocity * hitImpulse, ForceMode.Impulse);
            }
        }
        if(other.gameObject.tag != "Hands" && isSlashing && other.gameObject.layer != 6)
        {
            if(other.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector2 angles = CalcProgram.getAngleBetweenPoints3D(transform.position, camRig.centerEyeAnchor.position);
                other.gameObject.GetComponent<Rigidbody>().AddForce(CalcProgram.getVectorFromAngle3D(angles.x, angles.y, knockBackSlash), ForceMode.Impulse);
            }
           
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (scytheState == "grabbed")
        {
            rb.isKinematic = true;
        }
        if(scytheState == "flying")
        {
            rb.isKinematic = false;
            rb.velocity = lockedVelocity;
            rb.angularVelocity = Vector3.zero;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(xAxisChange, yAxisRotateOnRelease, zAxisRotateOnRelease));
            if (xChangePositive)
            {
                xAxisChange += Time.deltaTime * CalcProgram.getDist3D(lockedVelocity) * speedToSpinRatio;
            }
            else
            {
                xAxisChange -= Time.deltaTime * CalcProgram.getDist3D(lockedVelocity) * speedToSpinRatio;
            }
            
        }
        if(scytheState == "stuck")
        {
        }
        else
        {
            gameObject.transform.localScale = setSize;
        }
        if (isSlashing)
        {
            collide.size = slashSizeIncrease;
        }
        else
        {
            collide.size = slashOriginalSize;
        }
    }
}