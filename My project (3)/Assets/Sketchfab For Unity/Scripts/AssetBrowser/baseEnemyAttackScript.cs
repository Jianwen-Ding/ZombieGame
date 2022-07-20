using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEnemyAttackScript : MonoBehaviour
{
    [SerializeField]
    GameObject attackPrefab;
    [SerializeField]
    BaseEnemy enemyBase;
    [SerializeField]
    GameObject player;
    [SerializeField]
    float range;
    [SerializeField]
    float attackCoolDownLeft;
    [SerializeField]
    float attackCoolDown;
    [SerializeField]
    float distAway;
    [SerializeField]
    bool hasProjectileScript;

    // Start is called before the first frame update
    void Start()
    {
        hasProjectileScript = attackPrefab.GetComponent<DamagingProjectileScript>() != null;
        attackCoolDownLeft = attackCoolDown;
        enemyBase = gameObject.GetComponent<BaseEnemy>();
        player = PlayerMainScript.solePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = CalcProgram.getDistBetweenPoints3D(player.transform.position, gameObject.transform.position);
        if(enemyBase.getIsActivated() && enemyBase.getIsDead() == false && distance < range)
        {
            attackCoolDownLeft -= Time.deltaTime;
            if(attackCoolDownLeft <= 0)
            {
                Vector2 anglesToPlayer = CalcProgram.getAngleBetweenPoints3D(player.transform.position, gameObject.transform.position);
                GameObject projectile;
                if (distAway > 0)
                {
                    projectile = Instantiate(attackPrefab, CalcProgram.getVectorFromAngleBetweenTwoPoints3D(anglesToPlayer.x, anglesToPlayer.y, gameObject.transform.position, distAway), Quaternion.identity.normalized);
                    projectile.transform.position = CalcProgram.getVectorFromAngleBetweenTwoPoints3D(anglesToPlayer.x, anglesToPlayer.y, gameObject.transform.position, distAway);
                }
                else
                {
                    projectile = Instantiate(attackPrefab, gameObject.transform.position, Quaternion.identity.normalized);
                }
                
                if (hasProjectileScript)
                {
                    projectile.GetComponent<DamagingProjectileScript>().setDirection(anglesToPlayer);
                }
                attackCoolDownLeft = attackCoolDown;
            }
        }
    }
}
