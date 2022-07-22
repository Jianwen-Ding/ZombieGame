using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    PlayerMainScript playerScript;
    Teleporter teleScript;
    Text UIText;
    // Start is called before the first frame update
    void Start()
    {
        if (UIText == null) 
        {
            UIText = this.transform.GetChild(0).GetComponent<Text>();
        }
        if (teleScript == null) 
        {
            teleScript = GameObject.Find("Teleporter Placeholder Front").GetComponent<Teleporter>();
        }
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UIText != null) 
        {   
            UIText.text = "Health: " + playerScript.getHealth().ToString() + "\n" + "People Saved: " + teleScript.getCount();
        }
    }
}
