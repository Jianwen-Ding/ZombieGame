using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    PlayerMainScript playerScript;
    Text health;
    // Start is called before the first frame update
    void Start()
    {
        if (health == null) 
        {
            health = this.transform.GetChild(0).GetComponent<Text>();
        }
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health != null) 
        {   
            health.text = "Health: " + playerScript.getHealth().ToString();
        }
    }
}
