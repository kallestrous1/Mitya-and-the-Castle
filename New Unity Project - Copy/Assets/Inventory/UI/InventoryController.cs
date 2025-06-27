using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    bool inventoryActive = true;
    public GameObject inventory;
    CanvasGroup inventoryCanvas;
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
                inventoryCanvas.interactable = false;
                inventoryCanvas.alpha = 0;
                inventoryActive = false;
                inventory.SetActive(inventoryActive);
            }
            else
            {
                inventoryCanvas.interactable = true;
                inventoryCanvas.alpha = 1;
                inventoryActive = true;
                inventory.SetActive(inventoryActive);
            }
        }
    }
}
