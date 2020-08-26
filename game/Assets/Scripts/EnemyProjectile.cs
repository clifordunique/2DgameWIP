using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int damage = 0;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //So it doesnt slow the game down when it is still flying outside the map
        if (transform.position.magnitude > 100.0f)
        {
            Debug.Log("Projectile destroyed out of scene");
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        ProjectileOnCollisionWith(other.collider);
    }

    public void ProjectileOnCollisionWith(Collider2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("Ouchie");        
        }
        else if (enemy != null)
        {
            //wont hit itself and other enemies
            return;
        }
        Destroy(gameObject);

        Debug.Log("collision: " + other.gameObject);
    }

    public void Fired(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    public float GetDirection()
    {
        return transform.localScale.x;
    }
}
