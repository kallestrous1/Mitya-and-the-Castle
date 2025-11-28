using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    bool inventoryActive = true;
    public GameObject inventory;
    public GameObject shop;
    [SerializeField] private GameObject ItemDetailDisplay;
    CanvasGroup inventoryCanvas;

    public AudioClip openInventory;
    public AudioClip closeInventory;

    private void Start()
    {
        inventoryCanvas = inventory.GetComponent<CanvasGroup>();

    }

    protected void OnEnable()
    {
        InputManager.Instance.InventoryPressed += OnInventoryPressed;
    }

    protected void OnDisable()
    {
        InputManager.Instance.InventoryPressed -= OnInventoryPressed;
    }

    private void OnInventoryPressed()
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
        }
    }

    public void CloseItemDetailDisplay()
    {
        ItemDetailDisplay.SetActive(false);
    }
    public void OpenItemDetailDisplay()
    {
        ItemDetailDisplay.SetActive(true);
    }

}
