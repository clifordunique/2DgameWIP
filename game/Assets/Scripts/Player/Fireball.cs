using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int damage;
    Animator animator;
    //Start() does not work as the rigidbody2d is used the same frame it is created (when it is shot out by the player)

    //to look nicer
    public GameObject fireBallTrail;
    public float startTimeBtwSpawn;
    private float timeBtwSpawn;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {     
        //So it doesnt slow the game down when it is still flying outside the map
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
            //Debug.Log("destroyed out of scene");
        }
    }

    //While the original development uses edgecollider2D for the arrow, this is changed to boxcollider as it cannot collide with the enemy's edgecollider: https://answers.unity.com/questions/832350/no-collision-with-edge-collider-2d.html
    private void OnCollisionEnter2D(Collision2D other)
    {
        BasicEnemy enemy = other.gameObject.GetComponent<BasicEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(GetComponent<CircleCollider2D>());
        animator.SetTrigger("Impact");
        Destroy(gameObject, 3.0f);
        
    }

    public float GetDirection()
    {
        return transform.localScale.x;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}



//if (timeBtwSpawn <= 0)
//{
//    GameObject instance = Instantiate(fireBallTrail, transform.position+ new Vector3(Random.value - 0.5f, Random.value - 0.5f)/2, Quaternion.identity);
//    Destroy(instance, 3f);
//    timeBtwSpawn = startTimeBtwSpawn;
//}
//else
//{
//    timeBtwSpawn -= Time.deltaTime;
//}
