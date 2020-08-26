using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class FlyingEnemy : BasicEnemy
{
    public LayerMask layerGround;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        health = new HealthSystem(maxHealth);
        Debug.Log("FLIGHT");
        //originalColor = spriteRenderer.color;

        matWhite = Resources.Load("Effects/WhiteFlash", typeof(Material)) as Material;
        matOriginal = spriteRenderer.material;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }
    // Update is called once per frame
    void Update()
    {

        if (health.GetHealth() <= 0)
        {
            Die();
            Debug.Log("dead");
        }
    }
    public new void LookAtPlayer()
    {
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            isFlipped = true;
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            isFlipped = false;
        }
    }
    public float DistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, playerTransform.position);
    }
    public new void Die()
    {
        if (dead != true)
        {

            dead = true;
            animator.SetBool("Dead", true);

            //so it does get hit more, block projectiles and block player movement when the death animation is playing
            //GetComponent<BoxCollider2D>().enabled = false;
            Destroy(GetComponent<CapsuleCollider2D>());

            //so it does not fall under the map as boxcollider2d is now disabled
            //GetComponent<Rigidbody2D>().gravityScale = 0;
            Destroy(GetComponent<Rigidbody2D>());

            Invoke("Destroy", disappearingDelay);
        }
 
    
        }
 
}
