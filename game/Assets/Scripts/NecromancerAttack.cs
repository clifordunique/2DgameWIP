using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAttack : MonoBehaviour
{
    public float attackRangeMin;
    public float attackRangeMax;

    public Transform attackTransform;
    public Transform summonTransform;

    public int damage;

    public GameObject projectilePrefab;
    public GameObject[] summonableEnemies;

    public void Attack()
    {
        EnemyProjectile Projectile = Instantiate(projectilePrefab, attackTransform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        Projectile.damage = damage;

        //direction depends on the enemy's direction
        Projectile.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);
        Projectile.Fired(new Vector2(transform.localScale.x, 0), 400);
    }
    public void Summon()
    {
        GameObject summoned = Instantiate(summonableEnemies[0], summonTransform.position, Quaternion.identity);
    }
}
