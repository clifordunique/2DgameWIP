using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class PortalScript : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("next level");
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.NextLevel();
            Destroy(gameObject);
        }        
    }
}
