using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    private bool pickupRequest;

    private void Start()
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            equipment.GetSlots[i].OnBeforeUpdated += OnBeforeSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlotObject slot)
    {
        if(slot.ItemObject == null)
        {
            return;
        }

        switch (slot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                slot.ItemObject.UnequipItem();
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlotObject slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        switch (slot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                slot.ItemObject.EquipItem();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            pickupRequest = true;
        }

            if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Inventory saved from PlayerInventory");
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Inventory loaded from PlayerInventory");
            inventory.Load();
            equipment.Load();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var item = collision.GetComponent<ItemInGame>();
        if (item)
        {
            SuperItem thisItem = new SuperItem(item.item);
            if (pickupRequest)
            {
                pickupRequest = false;
                if (inventory.AddItem(thisItem))
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

}
