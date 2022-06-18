using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    bool inventoryActive;
    public GameObject inventory;
    CanvasGroup cg;
    private void Start()
    {
        cg = inventory.GetComponent<CanvasGroup>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryActive)
            {
                cg.interactable = false;
                cg.alpha = 0;
                inventoryActive = false;
            }
            else
            {
                cg.interactable = true;
                cg.alpha = 1;
                inventoryActive = true;
            }
        }
    }
}
