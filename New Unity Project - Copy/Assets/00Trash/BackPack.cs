using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BackPack", menuName = "Inventory/BackPack")]
public class BackPack : ScriptableObject
{
    new public string name;
    public Sprite icon = null;
    public InventorySlot[] slots = new InventorySlot[4];
    Inventory inventory;
    void OnEnable()
    {
        Debug.Log("cool from backpack");
        inventory = Inventory.instance;      
       
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i<slots.Length; i++)
        {
            Debug.Log("slot "+i +" : "+ slots[i].getItem());
            if (item.itemType.ToString() == slots[i].slotType.ToString() && slots[i].getItem()==null)
            {
                Debug.Log("adding " + item.name + " to slot " + i+ "of backpack" + this.name);
                slots[i].addItem(item);
                item.removeFromInventory();
                return;
            }
        }
    }

    public void Unequip(int slotIndex)
    {
        inventory = Inventory.instance;
        if (slots[slotIndex].getItem() != null)
        {
            Equipment oldItem = (Equipment)slots[slotIndex].getItem();
            Debug.Log("removing from backpack: " + oldItem.name);
            inventory.Add(oldItem);
            slots[slotIndex].clearSlot();
        }
    }
}
