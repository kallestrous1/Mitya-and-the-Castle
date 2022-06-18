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
        equipment.Load();
        inventory.Load();
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
        if (Input.GetButton("Interact"))
        {
            pickupRequest = true;
        }
        else
        {
            pickupRequest = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
            equipment.Load();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var item = collision.GetComponentInChildren<ItemInGame>();
        if (item)
        {
            if (pickupRequest)
            {
                SuperItem thisItem = new SuperItem(item.item);
                pickupRequest = false;
                if (inventory.AddItem(thisItem))
                {
                    Destroy(collision.transform.parent.gameObject);
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
