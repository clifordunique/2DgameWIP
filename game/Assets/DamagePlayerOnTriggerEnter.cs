using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnTriggerEnter : MonoBehaviour
{
    bool damageDealt =  false;
    float resetTime = 1.0f;
    public int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        Debug.Log("OTE");
        //making sure the same move doesnt hit the player twice
        if (!damageDealt)
        {
            player.TakeDamage(damage);
            damageDealt = true;
        }
        else if (damageDealt)
        {
            Invoke("ResetDamageDealt", resetTime);
            
        }
    }

    private void ResetDamageDealt()
    {
        damageDealt = false;
    }
}
