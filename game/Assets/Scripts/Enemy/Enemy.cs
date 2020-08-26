using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Color originalColor;

    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    protected Transform playerTransform;
    protected SpriteRenderer spriteRenderer;

    //to save the players location while teleporting is taking place
    protected Vector2 playerPos;

    public HealthSystem health;

    public GameObject Chest;

    public int damage;
    public float speed;
    public int maxHealth;       //saved as prefab of a monster
    public float dropRate;
    public bool canBeStaggered;

    public float distanceFromPlayer;

    //for attack
    public LayerMask layerPlayer;
    

    public bool attacking;
    public bool staggered;
    public bool dead;

    public readonly float disappearingDelay = 3;
    public readonly float backwardsSpeedMod = 0.6f;
    //public abstract void Attack();
    //public abstract void TracePlayer();

    public float enemySize;
    public LayerMask layerGround;

    //This is important as it allows AttackEnemy() to get health component from <Enemy> instead of checking for enemy type first
    public void TakeDamage(int damage)
    {
        FlashRed();
        //GetComponentInChildren<EnemyHPBarDirection>().gameObject.SetActive(true)
        HealthBarActiveFor3Seconds();
        if (!dead)
        {
            if (canBeStaggered == true)
            {
                animator.SetTrigger("Hurt");
            }
            health.Damage(damage);
        }
    }


    public void DamagePlayer(PlayerController player)
    {
        player.TakeDamage(damage);
    }

    public void DamagePlayer(PlayerController player,int customDamage)
    {
        player.TakeDamage(customDamage);
    }

    public void LookAtPlayer()
        {
        if (transform.position.x > playerTransform.position.x)
        {
            ChangeLookDirectionLeft();
        }
        else
        {
            ChangeLookDirectionRight();
        }
    }
        public void ChangeLookDirectionRight()
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        public void ChangeLookDirectionLeft()
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    //Grounded enemies which move towards the player til the enemy is at a certain range(determined by the enemy's attack range)
    //https://answers.unity.com/questions/1084574/vector3movetowards-on-y-axis-only-c.html



    public abstract void Attack();

    protected void FlashRed()
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", 0.07f);
    }
    protected void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }

    public void MoveTowardsPlayer()
    {
        if (transform.position.x > playerTransform.position.x)
        {
            rigidbody2d.velocity = new Vector2(-speed, rigidbody2d.velocity.y);
        }
        else
        {
            rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
        }
        
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y), speed * Time.deltaTime);
    }
    public void MoveAwayFromPlayer()
    {
        if (transform.position.x > playerTransform.position.x)
        {
            rigidbody2d.velocity = new Vector2(-backwardsSpeedMod * -speed, rigidbody2d.velocity.y);
        }
        else
        {
            rigidbody2d.velocity = new Vector2(-backwardsSpeedMod * speed, rigidbody2d.velocity.y);
        }
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y), -backwardsSpeedMod * speed * Time.deltaTime);
    }
    public void StopMovement()
    {
        rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
    }



    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Die()
    {
        if (dead != true)
        {
            float randomValue = UnityEngine.Random.value;           //Random float from 0 to 1
            if (randomValue <= dropRate)
            {
                DropChest();
            }
            dead = true;
            animator.SetTrigger("Death");

            //so it does get hit more, block projectiles and block player movement when the death animation is playing
            //GetComponent<BoxCollider2D>().enabled = false;
            Destroy(GetComponent<CapsuleCollider2D>());

            //so it does not fall under the map as boxcollider2d is now disabled
            //GetComponent<Rigidbody2D>().gravityScale = 0;
            Destroy(GetComponent<Rigidbody2D>());

            Invoke("Destroy", disappearingDelay);
        }
    }

    protected void HealthBarActiveFor3Seconds()
    {
        if (transform.GetChild(0).gameObject != null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            CancelInvoke("HealthBarSetActiveFalse");
            Invoke("HealthBarSetActiveFalse", 3.0f);
        }
        else
        {
            Debug.Log("No HealthBar Attached");
        }
        
    }

    protected void HealthBarSetActiveFalse()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }




    protected void SetMovingForAnimator()
    {
        if (GetVelocity().x != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
    public Vector2 GetLookDirectionVector2()
    {
        return new Vector2(transform.localScale.x, 0);
    }


    public float GetLookDirection()
    {
        return transform.localScale.x;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public Vector2 GetVelocity()
    {
        return rigidbody2d.velocity;
    }
    public void DropChest()
    {
            Instantiate(Chest, rigidbody2d.position, Quaternion.identity);
    }

    void PlayAudio(string audioName)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(audioName);
        }
    }
    public void StartTeleportToPlayer()
    {
        playerPos = playerTransform.position;
        animator.SetTrigger("TeleportP1");
        for (int i = 3; i == 0; i--)
        {
            if (CheckIfAvailable(GenerateRandomPositionNearPlayer(i)))
            {
                transform.position = GenerateRandomPositionNearPlayer(i);
                return;
            }
        }        
        animator.SetTrigger("TeleportP2");
    }
    private Vector2 GenerateRandomPositionNearPlayer (int distance)
    {
        //distance is used for efficiency
        return new Vector2(UnityEngine.Random.Range(playerPos.x - (float)distance, playerPos.x + (float)distance), playerTransform.position.y);        
    }
    private bool CheckIfAvailable(Vector2 x)
    {
        RaycastHit2D hit = Physics2D.Raycast(x, Vector2.down, 3f, layerGround);
        return hit.collider != null;
        // should also check if postion is inside a wall
        //https://answers.unity.com/questions/40996/check-if-position-is-inside-a-collider.html
    }
}
