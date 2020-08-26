using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAttack : MonoBehaviour
{
    public int damage;
    public Transform attackTransform;
    public LayerMask layerMaskPlayer;

    public float distanceTeleport;


    public float attackRange;
    public readonly float hitBoxRadius = 0.3f;
    public LayerMask layerGround;

    

    Vector2 playerPos;
    Vector2 randomPos; 
    BasicEnemy enemy;
    Animator animator;


    private void Start()
    {
        enemy = GetComponent<BasicEnemy>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    public void Attack()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(attackTransform.position, hitBoxRadius, layerMaskPlayer);
        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    public void TeleportToPlayer()
    {
            playerPos = enemy.playerTransform.position;

            randomPos = GenerateRandomPositionNearPlayer(1);

            if (CheckIfAvailable(randomPos))
            {
                //Debug.Log(playerPos);

                transform.position = randomPos;
                return;
            }
    }
        //transform.position = transform.position;


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, hitBoxRadius);
    }

    private Vector2 GenerateRandomPositionNearPlayer(int distance)
    {
        //distance is used for efficiency
        playerPos = enemy.playerTransform.position;
        return new Vector2(Random.Range(playerPos.x - (float)distance, playerPos.x + (float)distance), playerPos.y);
    }

    private bool CheckIfAvailable(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 3f, layerGround);
        //Debug.Log(hit.collider);
        //Debug.Log(Physics2D.OverlapCircle(pos, 0.1f, layerGround));
        return hit.collider != null && Physics2D.OverlapCircle(pos, 0.1f, layerGround) == null;
    }
}
