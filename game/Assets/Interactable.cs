using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey = KeyCode.F;
    public UnityEvent onInteract;
    public GameObject keyPopUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isInRange && keyPopUp.activeSelf == false)
        {
            Debug.Log("Key Active");
            keyPopUp.SetActive(true);
        }
        else if(!isInRange && keyPopUp.activeSelf == true)
        {
            Debug.Log("Key Inactive");
            keyPopUp.SetActive(false);
        }

        if (Input.GetKeyDown(interactKey) && isInRange)
        {
            keyPopUp.SetActive(false);
            onInteract.Invoke();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
