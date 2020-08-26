using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreAttack : MonoBehaviour
{
    public int damage;
    public Transform attackTransform;
    public LayerMask layerMaskPlayer;
    public float attackRange;
    public readonly float hitBoxRadius = 0.55f;

    public void Attack()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(attackTransform.position, hitBoxRadius, layerMaskPlayer);
        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, hitBoxRadius);
    }
    }
