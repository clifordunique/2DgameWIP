using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike : MonoBehaviour
{
    public int damage;

    public void EnabaleCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BasicEnemy>() != null)
        {
            collision.GetComponent<BasicEnemy>().TakeDamage(damage);
        }
    }
}
