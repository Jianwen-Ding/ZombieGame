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
        if (playerScript == null)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainScript>();
        }
        if (teleScript == null) 
        {
            teleScript = GameObject.Find("Teleporter Placeholder Front").GetComponent<Teleporter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UIText != null) 
        {   
            UIText.text = "Health: " + playerScript.getHealth().ToString();
        }
        if (teleScript != null) 
        {
            UIText.text += "\n" + "People Saved: " + teleScript.getCount();
        }
    }
}
