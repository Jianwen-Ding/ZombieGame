using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class PlayerMainScript : MonoBehaviour
{
    //Devices
    private InputDevice right;
    private InputDevice left;
    //gameObject
    public static GameObject solePlayer;
    [SerializeField]
    GameObject mainCamera;
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
    {   //establish devices
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        InputDeviceCharacteristics rightControllerChar = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerChar, inputDevices);
        if(inputDevices.Count > 0)
        {
            right = inputDevices[0];
        }
        InputDevices.GetDevices(inputDevices);
        InputDeviceCharacteristics leftControllerChar = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerChar, inputDevices);
        if (inputDevices.Count > 0)
        {
            left = inputDevices[0];
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
        
        //Inputs/Movement

    }
}
