using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    public ItemTracker itemTracker;

    private bool pickupRequest;

    private void Start()
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnAfterUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnBeforeUpdated += OnAddItem;
        }
        //inventory.Load();
       // equipment.Load();
        StartCoroutine(LoadInventoryOnStartup());
    }

    IEnumerator LoadInventoryOnStartup()
    {
        yield return new WaitForSeconds(1f);
        inventory.Load();
        equipment.Load();
        
    }

    public void OnAddItem(InventorySlotObject slot)
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
                slot.ItemObject.UnequipItem();
                break;
            default:
                break;
        }

    }

    public void OnRemoveItem(InventorySlotObject slot)
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
                    GetComponent<PlayerSound>().PlayPickUpItem();
                    itemTracker = FindAnyObjectByType<ItemTracker>();
                    itemTracker.itemsInGame.Remove(item.item);
                    itemTracker.collectedItems.Add(item);
                    item.isActive = false;
                    Destroy(collision.transform.parent.gameObject);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Save();
        equipment.Save();

    }

}
