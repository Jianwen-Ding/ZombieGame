using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
public class PlayerMainScript : MonoBehaviour
{
    //Devices
    OVRCameraRig cam = null;
    //gameObject
    public static GameObject solePlayer;

    Rigidbody rb;
    //scytheManagement
    [SerializeField]
    float scytheTimeNeeded;
    [SerializeField]
    GameObject rightScythe;
    [SerializeField]
    string rightScytheState;
    [SerializeField]
    GameObject leftScythe;
    [SerializeField]
    string leftScytheState;
    //Health Varibles
    private int health;
    private int maxHealth;
    [SerializeField]
    private float speed;
    public void damage(int damage)
    {
        health -= damage;
    }
    public void heal(int heal)
    {
        health += heal;
    }
    public int getHealth()
    {
        return health;
    }
    public void setMaxHealth(int newMax)
    {
        maxHealth = newMax;
    }
    public int getMaxHeath()
    {
        return maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        //establish camera
        OVRCameraRig[] CameraRigs = gameObject.GetComponentsInChildren<OVRCameraRig>();

        if (CameraRigs.Length == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (CameraRigs.Length > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            cam = CameraRigs[0];
        //establish self
        rb = gameObject.GetComponent<Rigidbody>();

        //establish sole player
        bool foundOthers = false;
        GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int x = 0; x < otherObjects.Length; x++)
        {
            if (otherObjects[x] != gameObject)
            {
                foundOthers = true;
                Destroy(gameObject);
            }
        }
        if(foundOthers == false)
        {
            solePlayer = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero)
        {
            float sideWaysAngle = CalcProgram.getAngleBetweenPoints2D(cam.rightEyeAnchor.position.x, cam.rightEyeAnchor.position.z, cam.leftEyeAnchor.position.x, cam.leftEyeAnchor.position.z);
            sideWaysAngle += 160;
            float angle = CalcProgram.getAngle2D(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y);
            rb.velocity = new Vector3(CalcProgram.getVectorFromAngle2D(angle + sideWaysAngle, speed).x, rb.velocity.y, CalcProgram.getVectorFromAngle2D(angle + sideWaysAngle, speed).y);

        }
        //Establishing controllers
        
    }
}
