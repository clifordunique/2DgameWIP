using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoboldAttack : MonoBehaviour
{
    public float basicAttackRange;
    public float dashAttackRangeMin;
    public float dashAttackRangeMax;
    public Transform attackTransform;

    public float attackDrag;
    public float originalDrag;  //make it same as rigidbody2d

    public LayerMask layerMaskPlayer;

    public readonly float hitBoxRadius = 0.4f;

    public int damage;
    public float dashForce;

    public GameObject attackCollider;

    private void Start()
    {
        originalDrag = GetComponent<Rigidbody2D>().drag;
    }

    public void EnableAttackCollider()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void DisableAttackCollider()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void DashAttack()
    {
        GetComponent<Rigidbody2D>().AddForce(GetComponent<BasicEnemy>().GetLookDirectionVector2() * dashForce);
    }
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
