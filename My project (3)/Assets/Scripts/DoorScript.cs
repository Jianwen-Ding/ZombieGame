using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Transform LeftDoor;
    private Transform RightDoor;
    public GameObject Player;

    public void open() {
        LeftDoor.localPosition = Vector3.left;
        RightDoor.localPosition = Vector3.right;
        //  TODO
        //  Make animation for opening/closing
        //
    }

    public void close() {
        LeftDoor.localPosition = Vector3.zero;
        RightDoor.localPosition = Vector3.zero;
    }

    void Start() {
        // Grab Objects through code rather than in the editor
        LeftDoor = transform.GetChild(2).transform.GetChild(0);
        RightDoor = transform.GetChild(2).transform.GetChild(1);
        Player = GameObject.FindWithTag("Player");
    }

    void Update() {
        if(Vector3.Distance(Player.transform.position, this.transform.position) < 5){
            open();
        } else {
            close();
        }
    }
}
