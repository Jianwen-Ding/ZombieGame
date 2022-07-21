using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
       
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 7)
        {
            
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
