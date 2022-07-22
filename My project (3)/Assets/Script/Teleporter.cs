using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    static int teleCount;
    GameObject player;
    Collider playerCollider;
    [SerializeField]
    float teleTimer;
    bool colliding;
    Material teleColor;

    public float timeUntilTP = 3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = GameObject.FindGameObjectWithTag("PlayerCollider").GetComponent<Collider>();
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
        if (colliding)
        {
            teleTimer += Time.deltaTime;
            //if player is colliding with the teleporter for over 3 seconds, send them to spawn
            if (teleTimer > timeUntilTP) {
                player.transform.position = new Vector3(0, -30, -25.6f);
                colliding = false;
                teleCount++;
            }
        } 
        else 
        {
            teleTimer = 0f;
        }
    }
}
