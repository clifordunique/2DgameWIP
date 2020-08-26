using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject attackCollider;
    Rigidbody2D rb;
    BasicEnemy enemy;
    Vector2 attackDirection;

    public float dashForce;
    public float attackDrag;
    public float originalDrag;
    public int damage;

    void Start()
    {
        enemy = GetComponentInParent<FlyingEnemy>();
        rb = GetComponentInParent<Rigidbody2D>();
        originalDrag = rb.drag;

    }


    public void DashAttack()
    {
        attackDirection = ((Vector2)enemy.playerTransform.position - rb.position).normalized;

        rb.AddForce(attackDirection * dashForce);
    }

    public void EnableAttackCollider()
    {
        attackCollider.SetActive(true);
    }
    public void DisableAttackCollider()
    {
        attackCollider.SetActive(false);
    }
    void LongFlashWhite()
    {
        enemy.FlashWhiteTimed(0.15f);
    }
}
