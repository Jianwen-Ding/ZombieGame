using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingProjectileScript : MonoBehaviour
{
    bool establishDir = false;
    Rigidbody rb;
    [SerializeField]
    int damage;
    [SerializeField]
    Vector2 direction = Vector2.zero;
    [SerializeField]
    float speed;
    [SerializeField]
    float lifeTime;
    public void setDirection(Vector2 angles)
    {
        direction = angles;
        establishDir = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            PlayerMainScript.solePlayer.GetComponent<PlayerMainScript>().damage(damage);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        if(speed > 0 && establishDir)
        {
            rb.velocity = CalcProgram.getVectorFromAngle3D(direction.x, direction.y, speed);
        }
        
    }
}
