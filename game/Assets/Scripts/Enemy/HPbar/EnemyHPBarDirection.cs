using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPBarDirection : MonoBehaviour
{
    BasicEnemy enemy;
  
    void Start()
    {
        enemy = GetComponentInParent<BasicEnemy>();
    }

    void Update()
    {
        if (enemy.isFlipped)
        {
            transform.localScale = new Vector3(-1f, 1f);
        }
        else if (!enemy.isFlipped)
        {
            transform.localScale = new Vector3(1f, 1f);
        }
    }
}
