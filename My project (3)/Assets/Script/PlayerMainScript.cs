using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class PlayerMainScript : MonoBehaviour
{
    //Devices
    [SerializeField]
    InputDevice rightContr;
    [SerializeField]
    InputDevice leftContr;
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
        //establish devices
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        InputDeviceCharacteristics rightControllerChar = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerChar, inputDevices);
        if(inputDevices.Count > 0)
        {
            rightContr = inputDevices[0];
        }
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
        InputDevices.GetDevices(inputDevices);
        InputDeviceCharacteristics leftControllerChar = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerChar, inputDevices);
        if (inputDevices.Count > 0)
        {
            leftContr = inputDevices[0];
        }
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
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
        Debug.Log(rightContr);
        //Inputs/Movement
        rightContr.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        Debug.Log(primaryButtonValue);
        if (primaryButtonValue)
        {

        }
        rightContr.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        Debug.Log(triggerValue);
        if (triggerValue > 0.1f)
        {

        }
        rightContr.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        Debug.Log(primary2DAxisValue);
        if (primary2DAxisValue != Vector2.zero)
        {
            float angle = CalcProgram.getAngle2D(primary2DAxisValue.x, primary2DAxisValue.y);
            rb.velocity = new Vector3(CalcProgram.getVectorFromAngle2D(mainCamera.transform.rotation.x + angle,speed).x, CalcProgram.getVectorFromAngle2D(mainCamera.transform.rotation.x + angle, speed).y, rb.velocity.z);
            
        }
    }
}
