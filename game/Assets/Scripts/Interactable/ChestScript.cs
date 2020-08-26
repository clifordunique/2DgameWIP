using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Animator animator;

    public Sprite openSprite, closedSprite;
    public GameObject[] chestItems;
    //temp
    public GameObject spawnedItem;
    public GameObject interactableCircle;
    //for testing
    public bool isOpen;

    private BoxCollider2D boxCollider2D;
    public GameObject pomf;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isOpen = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OpenChest()
    {
        if (isOpen == false)
        {
            isOpen = true;
            //so that the script does keep running (i.e. the F key popup will not show up when player in range)
            interactableCircle.SetActive(false);

            animator.SetTrigger("Open");
            //spriteRenderer.sprite = openSprite;
            Destroy(gameObject, 2.0f);
            Invoke("Pomf", 1.9f);

            spawnedItem = Instantiate(chestItems[Random.Range(0, chestItems.Length)], transform.position + Vector3.up * 0.2f, Quaternion.identity);

            spawnedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-300, 300), Random.Range(100, 400)));
        }
    }

    void Pomf()
    {
        Instantiate(pomf, transform.position, Quaternion.identity);
    }



}