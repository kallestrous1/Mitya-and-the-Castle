using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Rigidbody2D rb;
    bool pickUpRequest;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            pickUpRequest = true;
        }      
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (pickUpRequest == true)
        {
            Interactable item = other.GetComponent<Interactable>();
            if (item != null)
            {
                pickUpRequest = false;
                item.interact();
            }
        }
    }
}
