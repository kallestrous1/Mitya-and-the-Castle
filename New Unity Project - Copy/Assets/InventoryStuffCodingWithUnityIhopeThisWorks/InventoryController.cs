using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    bool inventoryActive;
    public GameObject inventory;
    private void Start()
    {
        inventory.SetActive(false);
        inventoryActive = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryActive)
            {
                inventoryActive = false;
                inventory.SetActive(false);
            }
            else
            {
                inventoryActive = true;
                inventory.SetActive(true);
            }
        }
    }
}
