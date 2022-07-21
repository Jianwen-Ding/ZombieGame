using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    GameObject player;
    Collider playerCollider;
    [SerializeField]
    float teleTimer;
    bool colliding;

    public float timeUntilTP = 3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = GameObject.FindGameObjectWithTag("PlayerCollider").GetComponent<Collider>();
        Debug.Log(playerCollider);
        teleTimer = 0f;
        colliding = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == playerCollider)
        {
            colliding = true;
        }
    }

    void OnTriggerLeave(Collider other)
    {
        if(other == playerCollider)
        {
            colliding = false;
        }
    }

    void Update()
    {
        if (colliding)
        {
            teleTimer += Time.deltaTime;
            //if player is colliding with the teleporter for over 3 seconds, send them to spawn
            if (teleTimer > timeUntilTP) {
                player.transform.position = new Vector3(0, -30, -25.6f);
                coliding = false;
            }
        } 
        else 
        {
            teleTimer = 0f;
        }
    }
}
