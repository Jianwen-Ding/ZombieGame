using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Teleporter : MonoBehaviour
{
    [SerializeField]
    static int teleCount;
    [SerializeField]
    Vector3 originalLocation;
    [SerializeField]
    GameObject player;
    Collider playerCollider;
    [SerializeField]
    float teleTimer;
    bool colliding;
    Material teleColor;
    [SerializeField]
    string teleGo;
    public float timeUntilTP = 3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = GameObject.FindGameObjectWithTag("PlayerCollider").GetComponent<Collider>();
        originalLocation = player.transform.position;
        Debug.Log(playerCollider);
        if (teleColor == null)
        {
            teleColor = transform.GetComponent<Renderer>().material;
        }
        teleTimer = 0f;
        teleCount = 0;
        colliding = false;

        teleColor.color = Color.red;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == playerCollider)
        {
            colliding = true;
            teleColor.color = Color.green;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other == playerCollider)
        {
            colliding = false;
            teleColor.color = Color.red;
        }
    }

    public int getCount() 
    {
        return teleCount;
    }

    void Update()
    {
        if(teleCount <= 2)
        {
            SceneManager.LoadScene(teleGo);
        }
        if (colliding)
        {
            teleTimer += Time.deltaTime;
            //if player is colliding with the teleporter for over 3 seconds, send them to spawn
            if (teleTimer > timeUntilTP) {
                Destroy(gameObject.transform.parent.gameObject);
                player.transform.position = originalLocation;
                colliding = false;
                teleCount++;
                ScytheGrab[] getGrab = player.gameObject.GetComponentsInChildren<ScytheGrab>();
                for (int x = 0; x < getGrab.Length; x++)
                {
                    getGrab[x].forceGrab();
                }
               
            }
        } 
        else 
        {
            teleTimer = 0f;
        }
    }
}
