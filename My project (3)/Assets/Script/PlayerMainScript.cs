using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class PlayerMainScript : MonoBehaviour
{
    //Devices
    //gameObject
    public static GameObject solePlayer;
    [SerializeField]
    GameObject mainCamera;
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

        }
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {

        }
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero)
        {
            float angle = CalcProgram.getAngle2D(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y);
            rb.velocity = new Vector3(CalcProgram.getVectorFromAngle2D(mainCamera.transform.rotation.x + angle, speed).x, rb.velocity.y, CalcProgram.getVectorFromAngle2D(mainCamera.transform.rotation.x + angle, speed).y);

        }
        //Establishing controllers

    }
}
