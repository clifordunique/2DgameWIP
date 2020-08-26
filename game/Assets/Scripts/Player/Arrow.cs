using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int damage = 0;
    public float arrowForce;

    //Start() does not work as the rigidbody2d is used the same frame it is created (when it is shot out by the player)
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //So it doesnt slow the game down when it is still flying outside the map
        if (transform.position.magnitude> 100.0f)
        {
            Destroy(gameObject);
            Debug.Log("Arrow destroyed out of scene");
        }

        //make the arrow's sprite direction the same as its velocity direction
        float angle = Mathf.Atan2(rigidbody2d.velocity.y, rigidbody2d.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //While the original development uses edgecollider2D for the arrow, this is changed to boxcollider as it cannot collide with the enemy's edgecollider: https://answers.unity.com/questions/832350/no-collision-with-edge-collider-2d.html
    private void OnCollisionEnter2D(Collision2D other)
    {
        BasicEnemy enemy = other.gameObject.GetComponent<BasicEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                PlayAudio("ArrowOnHitAudio1");
            }
            else
            {
                PlayAudio("ArrowOnHitAudio2");
            }
        }

        Destroy(gameObject);

        Debug.Log("Arrow collision: " + other.gameObject);
    }

    void PlayAudio(string audioName)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(audioName);
        }
    }
    public float GetDirection()
    {
        return transform.localScale.x;
    }

}
