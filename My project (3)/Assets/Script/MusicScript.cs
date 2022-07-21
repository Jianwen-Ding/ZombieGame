using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public static MusicScript soleMusic;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allMusic = GameObject.FindGameObjectsWithTag("Music");
        if(allMusic.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            soleMusic = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
