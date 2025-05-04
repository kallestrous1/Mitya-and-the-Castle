using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInteractable : MonoBehaviour
{
    public GameObject textBox;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInteract player = collision.gameObject.GetComponent<PlayerInteract>();
        if (player != null)
        {
            textBox.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInteract player = collision.gameObject.GetComponent<PlayerInteract>();
        if (player != null)
        {
            textBox.SetActive(false);
        }
    }
}
