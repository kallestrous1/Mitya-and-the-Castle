using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    bool inventoryActive = true;
    public GameObject inventory;
    public GameObject shop;
    CanvasGroup inventoryCanvas;

    public AudioClip openInventory;
    public AudioClip closeInventory;

    private void Start()
    {
        inventoryCanvas = inventory.GetComponent<CanvasGroup>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryActive)
            {
                if (closeInventory)
                {
                    AudioManager.Instance.Play(closeInventory);
                }
                inventoryCanvas.interactable = false;
                inventoryCanvas.alpha = 0;
                inventoryActive = false;
                inventory.SetActive(inventoryActive);
                shop.SetActive(inventoryActive);
            }
            else
            {
                if (openInventory)
                {
                    AudioManager.Instance.Play(openInventory);
                }
                inventoryCanvas.interactable = true;
                inventoryCanvas.alpha = 1;
                inventoryActive = true;
                inventory.SetActive(inventoryActive);
                shop.SetActive(inventoryActive);
            }
        }
    }
}
