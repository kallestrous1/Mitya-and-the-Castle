using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    public ItemTracker itemTracker;

    public BoneCombiner boneCombiner;

    private bool pickupRequest;

    private Transform charm;
    private Transform weapon;
    private Transform spell;

    ItemType type = default;

    private void Start()
    {
        boneCombiner = new BoneCombiner(gameObject);
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnAfterUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnBeforeUpdated += OnAddItem;
        }
        inventory.Load();
        equipment.Load();
    }

    public void OnAddItem(InventorySlotObject slot)
    {

        if (slot.ItemObject)
        {
            type = slot.ItemObject.type;
        }

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
        else
        {
            type = slot.ItemObject.type;
        }

        switch (slot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                slot.ItemObject.EquipItem();
                if (slot.ItemObject.characterDisplay != null)
                {
                    switch (type)
                    {
                        case ItemType.Weapon:
                            weapon = boneCombiner.AddLimb(slot.ItemObject.characterDisplay);
                            break;
                        case ItemType.Spell:
                            spell = boneCombiner.AddLimb(slot.ItemObject.characterDisplay);
                            break;
                        case ItemType.Charm:
                            charm = boneCombiner.AddLimb(slot.ItemObject.characterDisplay);
                            break;
                        default:
                            break;
                    }
                }
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
                    itemTracker = FindObjectOfType<ItemTracker>();
                    itemTracker.itemsInGame.Remove(item.item);
                    itemTracker.collectedItems.Add(item.item);
                    item.item.setActive = false;
                    Destroy(collision.transform.parent.gameObject);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Save();
        equipment.Save();
        inventory.Clear();
        equipment.Clear();
    }

}
