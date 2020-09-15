using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Abomination : BasicEnemy
{
    // Start is called before the first frame update
    CapsuleCollider2D weakspotcol;
    BoxCollider2D col;
    void Start()
    {
        weakspotcol = GetComponent<CapsuleCollider2D>();
        col = GetComponent<BoxCollider2D>();

        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = new HealthSystem(maxHealth);

        //originalColor = spriteRenderer.color;

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
    public void ChangeColliderToWeakSpot()
    {
        col.enabled = false;
        weakspotcol.enabled = true;
        canBeStaggered = true;
    }
    public void ChangeColliderToNormal()
    {
        col.enabled = true;
        weakspotcol.enabled = false;
        canBeStaggered = false;
    }
    public new void Die()
    {
        if (dead != true)
        {
            dead = true;
            animator.SetBool("Dead", true);

            //so it does get hit more, block projectiles and block player movement when the death animation is playing
            //GetComponent<BoxCollider2D>().enabled = false;
            Destroy(col);
            Destroy(weakspotcol);

            //so it does not fall under the map as boxcollider2d is now disabled
            //GetComponent<Rigidbody2D>().gravityScale = 0;
            Destroy(rigidbody2d);

            Invoke("Destroy", disappearingDelay);
        }
    }
    public new void TakeDamage(int damage)
    {
        FlashWhite();
        CreateBloodParticles();
        HealthBarActiveFor3Seconds();
        if (!dead)
        {
            if (canBeStaggered)
            {
                health.Damage(damage*=2);
                animator.SetTrigger("Hurt");
            }
            else
            {
                health.Damage(damage);
            }       
            GameObject damageText = Instantiate(DamageText, transform.position, Quaternion.identity);
            damageText.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            if (transform.position.x > playerTransform.position.x)
            {
                damageText.transform.GetChild(0).GetComponent<Animator>().SetBool("Right", true);
            }
            else
            {
                damageText.transform.GetChild(0).GetComponent<Animator>().SetBool("Right", false);
            }

        }
    }
}
