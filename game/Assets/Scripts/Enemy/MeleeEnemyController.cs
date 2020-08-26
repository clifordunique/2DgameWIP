using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : Enemy      //concrete*
{
    public float attackRange;
    public float range;
    public Transform attackPos;


    //for others, would be initialized to (int)AttackStyle.Melee at start()

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        attacking = false;

        health = new HealthSystem(maxHealth);

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        Debug.Log(originalColor);      
    }

    // Update is called once per frame
    void Update()
    {

        if (playerTransform != null)
        {            
            if (health.GetHealth() > 0)
            {
                MeleeEnemyBehaviour();               
            }

            else if (health.GetHealth() <= 0)
            {
                Die();
            }
        }
    }

    public override void Attack()
    {
        if (!attacking)
        {
            StopMovement();
            animator.SetTrigger("Attack");
        }
    }

 //   override public void TakeDamage(int damage)
 //   {
 //       FlashRed();
 //       if (!dead)
 //       {
 //           if (canBeStaggered == true)
 //           {
 //               animator.SetTrigger("Hurt");
 //           }
 //           health.Damage(damage);
       
 //}
  //  }
    public void MeleeAttack()
    {
        Collider2D playerCollider = PlayerColliderInFront();
        if (playerCollider != null)
        { 
            PlayerController playerToDamage = playerCollider.GetComponent<PlayerController>();
            if (playerToDamage != null)
                {
            DamagePlayer(playerToDamage);
                }
        }
    }

    public Collider2D PlayerColliderInFront()
    {
        return Physics2D.OverlapCircle(attackPos.position, attackRange, layerPlayer);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    //Ground
    void MeleeEnemyBehaviour()
    {

        distanceFromPlayer = Mathf.Abs(transform.position.x - playerTransform.position.x);
        bool playerGrounded = playerTransform.GetComponent<PlayerController>().grounded;

        SetMovingForAnimator();

        if (!attacking)
        {
            LookAtPlayer();
        }
        

        if (staggered || attacking)
        {
            return;
        }
        SetMovingForAnimator();

        if (distanceFromPlayer > range)
        {

            MoveTowardsPlayer();
        }
        //add another for when player is not on ground(wont att)
        else if (distanceFromPlayer <= range && playerGrounded)
        {
            Attack();
            StartTeleportToPlayer();
        }
        else
        {
            animator.SetBool("Idle", true);
        }
    }

}
