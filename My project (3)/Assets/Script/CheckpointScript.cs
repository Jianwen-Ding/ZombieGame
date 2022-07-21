using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    PlayerMainScript get;
    
    // Start is called before the first frame update
    void Start()
    {
        get = PlayerMainScript.solePlayer.GetComponent<PlayerMainScript>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 7)
        {
            get.heal(100);
            ScytheGrab[] getGrab= get.gameObject.GetComponentsInChildren<ScytheGrab>();
            for(int x = 0; x < getGrab.Length; x++)
            {
                getGrab[x].forceGrab();
            }
        }
    }
}
