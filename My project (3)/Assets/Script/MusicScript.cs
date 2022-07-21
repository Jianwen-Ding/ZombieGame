using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public static MusicScript soleMusic;
    AudioSource audioSource;
    public AudioSource getAudio()
    {
        return audioSource;
    }
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
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
