using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    //establish
    [SerializeField]
    GameObject player;
    [SerializeField]
    MeshRenderer render;
    //health
    [SerializeField]
    int health;
    [SerializeField]
    int maxHealth;
    bool isDead;
    //materials
    [SerializeField]
    Material baseMat;
    [SerializeField]
    Material damagedMat;
    [SerializeField]
    float timeDamageFlashedLeft;
    [SerializeField]
    float timeDamageFlashed;
    [SerializeField]
    Material deadMat;
    //Activation
    [SerializeField]
    bool activated;
    [SerializeField]
    float activationRange;
    
    public void setActivation(bool set)
    {
        activated = set;
    }
    public void damage(int damage)
    {
        health -= damage;
        render.material = damagedMat;
        timeDamageFlashedLeft = timeDamageFlashed;
        if(health < 0)
        {
            isDead = true;
            render.material = deadMat;
        }
    }
    public bool getIsActivated()
    {
        return activated;
    }
    public bool getIsDead()
    {
        return isDead;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMainScript.solePlayer;
        render = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activated == false)
        {
            if(Mathf.Abs(CalcProgram.getDistBetweenPoints3D(player.transform.position, gameObject.transform.position)) < activationRange)
            {
                activated = true;
            }
        }
        else
        {
            if (isDead)
            {
                render.material = deadMat;
                if (Mathf.Abs(CalcProgram.getDistBetweenPoints3D(player.transform.position, gameObject.transform.position)) > activationRange)
                {
                    activated = false;
                    isDead = false;
                    health = maxHealth;
                    render.material = baseMat;
                }

            }
            else
            {
                if (timeDamageFlashedLeft <= 0)
                {
                    render.material = baseMat;
                }
                else
                {
                    timeDamageFlashedLeft -= Time.deltaTime;
                }
            }
            
            
        }
    }
}
