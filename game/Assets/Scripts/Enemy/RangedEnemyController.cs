using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : Enemy
{

    public float range;

    public EnemyProjectile projectilePrefab;

    //public float distanceFromPlayer;

    //rangedMovement
    public float minRange;
    public float maxRange;

    public float deadZoneRange;

    public override void Attack()
    {
        animator.SetTrigger("Attack");
    }

    // Start is called before the first frame update
    void Start()
    {
        minRange = range - deadZoneRange;
        maxRange = range;

        health = new HealthSystem(maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        originalColor = spriteRenderer.color;

        attacking = false;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            distanceFromPlayer = Mathf.Abs(transform.position.x - playerTransform.position.x);
        }
        if (playerTransform != null && staggered != true && attacking != true)
        {
           
            if (health.GetHealth() > 0)
            {
                RangedMovementBehaviour();
            }

            if (health.GetHealth() <= 0)
            {
                Die();
            }
        }
    }

    //move to range to attack, move away from player
    protected void RangedMovementBehaviour()
    {

        bool playerGrounded = playerTransform.GetComponent<PlayerController>().grounded;
        SetMovingForAnimator();
        LookAtPlayer();

        if (attacking || staggered)
        {
            //movementState = MovementState.NotMoving;
            return;
        }

        if (distanceFromPlayer > maxRange)
        {
            MoveTowardsPlayer();
        }
        //add another for when player is not on ground(wont att)
        else if (distanceFromPlayer >= minRange && distanceFromPlayer <= maxRange)
        {
            StopMovement();
            Attack();
        }
        else if (distanceFromPlayer < minRange)
        {
            
            MoveAwayFromPlayer();
        }
        else
        {
            animator.SetBool("Idle", true);
        }
    }

    public void FireProjectile()
    {
        EnemyProjectile Projectile = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.1f, Quaternion.identity).GetComponent<EnemyProjectile>();
        Projectile.damage = damage;

        //direction depends on the enemy's direction
        Projectile.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);

        Projectile.Fired(new Vector2(transform.localScale.x, 0), 400);
        //throw new NotImplementedException();
    }
    void xd()
    {
        if (Mathf.Abs(playerTransform.position.y - transform.position.y) > 3f)
        {

        }
    }

}
