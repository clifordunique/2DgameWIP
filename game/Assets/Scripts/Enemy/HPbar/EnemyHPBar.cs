using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPBar : MonoBehaviour
{
    BasicEnemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<BasicEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
       transform.localScale = new Vector3(enemy.health.GetHealthPercentage(), 1f);
        
    }
}
