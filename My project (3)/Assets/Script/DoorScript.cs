using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject LeftDoor;
    public GameObject RightDoor;
    public GameObject Player;

    public void open() {
        LeftDoor.transform.localPosition = Vector3.left;
        RightDoor.transform.localPosition = Vector3.right;
        //  TODO
        //  Make animation for opening/closing
        //
    }

    public void close() {
        LeftDoor.transform.localPosition = Vector3.zero;
        RightDoor.transform.localPosition = Vector3.zero;
    }

    void Start() {
        // Grab Objects through code rather than in the editor
        //LeftDoor = ;
        //RightDoor = ;
        //Player = ;
    }

    void Update() {
        if(Vector3.Distance(Player.transform.position, this.transform.position) < 5){
            open();
        } else {
            close();
        }
    }
}
