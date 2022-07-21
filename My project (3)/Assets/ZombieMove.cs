using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    BaseEnemy baseE;
    GameObject player;
    Rigidbody Erb;
    [SerializeField]
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        baseE = gameObject.GetComponent<BaseEnemy>();
        player = PlayerMainScript.solePlayer;
        Erb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (baseE.getIsActivated() && baseE.getIsDead() == false)
        {
            float angle = CalcProgram.getAngleBetweenPoints3D(player.transform.position, gameObject.transform.position).x;
            Vector3 set = CalcProgram.getVectorFromAngle3D(angle, 0, speed);
            Erb.AddForce(set * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}
