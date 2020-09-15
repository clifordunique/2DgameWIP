using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicEnemy : MonoBehaviour
{
    public HealthSystem health;
    private Color originalColor;
    public GameObject healthBar;

    //whiteflash when damaged
    protected Material matWhite;
    protected Material matOriginal;

    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    
    protected SpriteRenderer spriteRenderer;

    //to save the players location while teleporting is taking place
    protected Vector2 playerPos;
    public GameObject chest;
    public ParticleSystem bloodEffects;
    public Transform playerTransform;

    public float speed;
    public int maxHealth;       //saved as prefab of a monster
    public float dropRate;
    public bool canBeStaggered;
    //public float distanceFromPlayer;

    public bool staggered;
    public bool dead;

    public readonly float disappearingDelay = 2;
    public readonly float backwardsSpeedMod = 0.5f;

    public float enemySize;

    public bool isFlipped = false;

    public GameObject DamageText;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = new HealthSystem(maxHealth);

        //originalColor = spriteRenderer.color;

        //https://www.youtube.com/watch?v=WgLd6EahyVU
        matWhite = Resources.Load("Effects/WhiteFlash", typeof(Material)) as Material;
        matOriginal = spriteRenderer.material;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (health.GetHealth() <= 0)
        {
            Die();
            Debug.Log("dead");
        }
    }


    //This is important as it allows AttackEnemy() to get health component from <Enemy> instead of checking for enemy type first
    public void TakeDamage(int damage)
    {
        FlashWhite();
        CreateBloodParticles();
        HealthBarActiveFor3Seconds();
        if (!dead)
        {
            health.Damage(damage);
            GameObject damageText = Instantiate(DamageText,transform.position, Quaternion.identity);
            damageText.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            if (transform.position.x > playerTransform.position.x)
            {
                damageText.transform.GetChild(0).GetComponent<Animator>().SetBool("Right", true);
            }
            else
            {
                damageText.transform.GetChild(0).GetComponent<Animator>().SetBool("Right", false);
            }
            if (canBeStaggered == true)
            {
                animator.SetTrigger("Hurt");
            }         
        }
    }

    public float DistanceFromPlayerYAxis()
    {
        return Mathf.Abs(playerTransform.position.y - transform.position.y);
    }
    public float DistanceFromPlayerXAxis()
    {
        return Mathf.Abs(playerTransform.position.x - transform.position.x);
    }

    public void DamagePlayer(PlayerController player, int customDamage)
    {
        player.TakeDamage(customDamage);
    }

    public void LookAtPlayer()
    {

        if (transform.position.x > playerTransform.position.x && !isFlipped)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            //transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
        else if (transform.position.x <= playerTransform.position.x && isFlipped)
        {
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
            //transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
    }

    //https://answers.unity.com/questions/1084574/vector3movetowards-on-y-axis-only-c.html

    protected void FlashWhite()
    {
        //used to show that the enemy is damaged
        spriteRenderer.material = matWhite;
        Invoke("ResetColor", 0.07f);
    }
    public void FlashWhiteTimed(float time)
    {
        //used to show that the enemy is about to do a powerful attack
        spriteRenderer.material = matWhite;
        Invoke("ResetColor", time);
    }


    protected void ResetColor()
    {
        //spriteRenderer.color = originalColor;
        spriteRenderer.material = matOriginal;
    }

    //protected void FlashRed()
    //{
    //    spriteRenderer.color = Color.red;
    //    Invoke("ResetColor", 0.07f);
    //}

    //movement
    //public void ChangeLookDirectionRight()
    //{
    //    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    //}
    //public void ChangeLookDirectionLeft()
    //{
    //    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
    //}

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

    public float getDistanceFormPlayer()
    {
        return Mathf.Abs(playerTransform.position.x - transform.position.x);
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
                Debug.Log("DropChest?");
            }
            dead = true;
            animator.SetBool("Dead", true);

            //so it does get hit more, block projectiles and block player movement when the death animation is playing
            //GetComponent<BoxCollider2D>().enabled = false;
            Destroy(GetComponent<CapsuleCollider2D>());

            //so it does not fall under the map as boxcollider2d is now disabled
            //GetComponent<Rigidbody2D>().gravityScale = 0;
            Destroy(rigidbody2d);

            Invoke("Destroy", disappearingDelay);
        }
    }

    protected void HealthBarActiveFor3Seconds()
    {
        if (healthBar != null)
        {
            healthBar.SetActive(true);
            Debug.Log("HP ACTIVE");
            CancelInvoke("HealthBarSetActiveFalse");
            Invoke("HealthBarSetActiveFalse", 3.0f);
        }
        else
        {
            Debug.Log("No HealthBar Attached");
        }

    }
    protected void CreateBloodParticles()
    {

        ParticleSystem bloodSplatter = Instantiate(bloodEffects, transform.position, Quaternion.Euler(-90f,0f,0f));
        ParticleSystem.ShapeModule psShape = bloodSplatter.shape;
        psShape.rotation = new Vector3(0f, 75f * playerTransform.localScale.x, 0f);
    }

    protected void HealthBarSetActiveFalse()
    {
        healthBar.SetActive(false);
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
        Instantiate(chest, transform.position, Quaternion.identity);
    }

    void PlayAudio(string audioName)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(audioName);
        }
    }

}
