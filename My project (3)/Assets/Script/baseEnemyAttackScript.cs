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
        float distAway = CalcProgram.getDistBetweenPoints3D(player.transform.position, gameObject.transform.position);
        if(enemyBase.getIsActivated() && enemyBase.getIsDead() && distAway < range)
        {
            attackCoolDownLeft -= Time.deltaTime;
            if(attackCoolDownLeft <= 0)
            {
                Vector2 anglesToPlayer = CalcProgram.getAngleBetweenPoints3D(player.transform.position, gameObject.transform.position);
                GameObject projectile = Instantiate(attackPrefab, CalcProgram.getVectorFromAngleBetweenTwoPoints3D(anglesToPlayer.x , anglesToPlayer.y, gameObject.transform.position, distAway), Quaternion.identity.normalized);
            }
        }
    }
}
