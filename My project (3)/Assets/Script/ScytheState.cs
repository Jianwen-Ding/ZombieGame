using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheState : MonoBehaviour
{
    //audio
    [SerializeField]
    AudioSource audioS;
    [SerializeField]
    AudioClip hitSound;
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
    [SerializeField]
    float flyingTimeTillGet;
    [SerializeField]
    float flyingTimeTillGetLeft;
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
    Vector3 addVelocity;
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
    int damageThrowDeal;
    [SerializeField]
    float minimumThrowSpeed;
    [SerializeField]
    float throwSpeedMultiplier;
    //stuck scytheState
    GameObject stabbedObject;
    [SerializeField]
    float hitImpulse;
    //slash
    [SerializeField]
    bool hasSlashedBefore = false;
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
    public void setHasSlashed(bool set)
    {
        hasSlashedBefore = set;
    }
    public string getState()
    {
        return scytheState;
    }
    public void setAddVel(Vector3 add)
    {
        addVelocity = add;
    }
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
        addVelocity = Vector2.zero;
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
            BaseEnemy potBEnemy = other.gameObject.GetComponent<BaseEnemy>();
            if (potBEnemy != null)
            {
                potBEnemy.damage(damageThrowDeal);
            }
            
        }
        if(other.gameObject.tag != "Hands" && isSlashing && other.gameObject.layer != 6)
        {
            if(other.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector2 angles = CalcProgram.getAngleBetweenPoints3D(transform.position, camRig.centerEyeAnchor.position);
                other.gameObject.GetComponent<Rigidbody>().AddForce(CalcProgram.getVectorFromAngle3D(angles.x, angles.y, knockBackSlash), ForceMode.Impulse);
                
            }
            BaseEnemy potBEnemy = other.gameObject.GetComponent<BaseEnemy>();
            if (potBEnemy != null)
            {
                potBEnemy.damage(damageOnSlash);
                hasSlashedBefore = true;
                if (hitSound != null)
                {
                    audioS.clip = hitSound;
                    audioS.time = 0;
                    audioS.Play();
                }
            }
           

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (hasSlashedBefore == false && other.gameObject.tag != "Hands" && isSlashing && other.gameObject.layer != 6 && other.gameObject.layer != 9)
        {
            if (other.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector2 angles = CalcProgram.getAngleBetweenPoints3D(transform.position, camRig.centerEyeAnchor.position);
                other.gameObject.GetComponent<Rigidbody>().AddForce(CalcProgram.getVectorFromAngle3D(angles.x, angles.y, knockBackSlash), ForceMode.Impulse);

            }
            BaseEnemy potBEnemy = other.gameObject.GetComponent<BaseEnemy>();
            if (potBEnemy != null)
            {
                potBEnemy.damage(damageOnSlash);
                hasSlashedBefore = true;
                if (hitSound != null)
                {
                    audioS.clip = hitSound;
                    audioS.time = 0;
                    audioS.Play();
                }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (scytheState == "grabbed")
        {
            
        }
        if(scytheState == "flying")
        {
            flyingTimeTillGetLeft -= Time.deltaTime;
            if(flyingTimeTillGetLeft < 0)
            {
                grabbedBy.GetComponent<ScytheGrab>().forceGrab();
            }
            rb.isKinematic = false;
            if(addVelocity == Vector3.zero)
            {
                rb.velocity = lockedVelocity;
            }
            else
            {
                rb.velocity = lockedVelocity + addVelocity;
                addVelocity = Vector3.zero;
            }
           
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
        else
        {
            addVelocity = Vector3.zero;
            flyingTimeTillGetLeft = flyingTimeTillGet;
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
