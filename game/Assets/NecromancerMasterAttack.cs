using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerMasterAttack : MonoBehaviour
{
    //deciders
    public float meleeAttackRange;

    [Header("Melee Attack")]
    public int damage;
    public Transform attackTransform;
    public LayerMask layerMaskPlayer;
    public readonly float hitBoxRadius = 0.55f;

    [Header("Summon")]
    public Transform summonTransform;
    public GameObject[] summonableEnemies;
    private int noOfSummonableEnemies;
    
    [Header("RangedAttack")]
    public GameObject projectilePrefab;
    public Transform projectileTransform;

    private void Start()
    {
        noOfSummonableEnemies = summonableEnemies.Length;
    }
    public void Attack()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(attackTransform.position, hitBoxRadius, layerMaskPlayer);
        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
    public void FireProjectile()
    {
        EnemyProjectile Projectile = Instantiate(projectilePrefab, projectileTransform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        Projectile.damage = damage;

        //direction depends on the enemy's direction
        Projectile.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);
        Projectile.Fired(new Vector2(transform.localScale.x, 0), 600);
        
    }

    public void Summon()
    {
        GameObject summoned = Instantiate(summonableEnemies[Random.Range(0, noOfSummonableEnemies)], summonTransform.position, Quaternion.identity);
    }


}
