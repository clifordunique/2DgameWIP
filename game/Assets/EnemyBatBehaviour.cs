using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyBatBehaviour : StateMachineBehaviour
{
    Transform playerTransform;
    public float speed = 3f;
    public float nextWpDistance = 1f;

    Path path;
    int currentWp = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    FlyingEnemy enemy;

    float timeBtwUpdate = 0f;
    float startTimeBtwUpdate = 0.5f;

    float timeBeforeAttack;
    float startTimeBeforeAttack = 2.0f;

    Vector2 direction;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<FlyingEnemy>();
        rb = animator.GetComponentInParent<Rigidbody2D>();
        playerTransform = enemy.playerTransform;
        seeker = animator.GetComponentInParent<Seeker>();
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        timeBeforeAttack = startTimeBeforeAttack;

    }
    
    void UpdatePath()
    {
        seeker.StartPath(rb.position, playerTransform.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWp = 0;
        }
    }
    

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();
        Vector2 attackDirection = (rb.position - (Vector2)playerTransform.position).normalized;
        if (enemy.DistanceFromPlayer() < 4f && Physics2D.Raycast(rb.position, attackDirection, 3.5f, enemy.layerGround).collider == null && timeBeforeAttack <= 0)  //attack rang echange to
        {
            Debug.Log(Physics2D.Raycast(rb.position, attackDirection, 3.5f, enemy.layerGround).collider == null);
            animator.SetTrigger("Attack");
        }
        timeBeforeAttack -= Time.deltaTime;

        if (timeBtwUpdate <= 0)
        {
            UpdatePath();
            timeBtwUpdate = startTimeBtwUpdate;
        }
        else
        {
            timeBtwUpdate -= Time.deltaTime;
        }

        if (path == null)
        {
            return;
            
        }
        if (currentWp >= path.vectorPath.Count)
        {
            Debug.Log("ReachedEnd");
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWp] - rb.position).normalized;
        rb.velocity = direction * speed;
        //Debug.Log("adadadad");

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWp]);

        if (distance < nextWpDistance)
        {
            currentWp++;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
