using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Shop
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]

public class InventoryObject : ScriptableObject
{
    public InterfaceType interfaceType;
    public string savePath;
    public ItemDataBaseObject database;
    public SuperInventory Container;
    public InventorySlotObject[] GetSlots{ get {return Container.Slots;}}

    public bool IsFull
    {
        get
        {
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item == null)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool AddItem(ItemData item)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }
      //  if we add stacks: something like if find item is null or if item is not stackable
      // create a new slot, otherwise add to find item slot 
      //  InventorySlotObject slot = FindItemOnInventory(item);
        setEmptySlot(item);
        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (string.IsNullOrEmpty(GetSlots[i].item.itemID))
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    //this is to find if the same item is already in the inventory, useful for stacking or something, but I don't need it rn
    public InventorySlotObject FindItemOnInventory(ItemData item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            var slot = GetSlots[i];
            if (slot.item != null && slot.item.itemID == item.itemID)
                return slot;
        }
        return null;
    }
    public InventorySlotObject setEmptySlot(ItemData item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == null || string.IsNullOrEmpty(GetSlots[i].item.itemID))
            {
                GetSlots[i].UpdateSlot(item);
                return GetSlots[i];
            }
        }
        // what if inventory is full?
        return null;
    }

    public void SwapItems(InventorySlotObject draggedObj, InventorySlotObject itemTwo)
    {
        if (itemTwo.CanPlaceInSlot(draggedObj.ItemObject) && draggedObj.CanPlaceInSlot(itemTwo.ItemObject)) 
        {
            InventorySlotObject temp = new InventorySlotObject(itemTwo.item);
            if(draggedObj.parent.inventory.interfaceType == itemTwo.parent.inventory.interfaceType)
            {
                itemTwo.UpdateSlot(draggedObj.item, true);
                draggedObj.UpdateSlot(temp.item, true);
            }
            else
            {
                itemTwo.UpdateSlot(draggedObj.item);
                draggedObj.UpdateSlot(temp.item);
            }

        }
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item == item)
            {
                GetSlots[i].UpdateSlot(null);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        var path = string.Concat(Application.persistentDataPath, savePath);
        if (!File.Exists(path)) return;

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        SuperInventory newContainer = (SuperInventory)formatter.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < GetSlots.Length; i++)
        {
            InventorySlotObject savedSlot = newContainer.Slots[i];

            savedSlot ??= newContainer.Slots[i] = new InventorySlotObject(new ItemData());
            savedSlot.item ??= new ItemData();

            // Convert legacy empty slot or no GUID case
            if (string.IsNullOrEmpty(savedSlot.item.name))
            {               
                GetSlots[i].UpdateSlot(new ItemData(), savedSlot.locked, savedSlot.price);
            }
            else if(string.IsNullOrEmpty(savedSlot.item.itemID))
            {
                GetSlots[i].UpdateSlot(savedSlot.item, savedSlot.locked, savedSlot.price);
                Debug.Log(database.GetItemByName(savedSlot.item.name));
                savedSlot.item.itemID = database.GetItemByName(savedSlot.item.name).itemID;
            }

            GetSlots[i].UpdateSlot(savedSlot.item, savedSlot.locked, savedSlot.price);

        }

    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

    [ContextMenu("Default")]
    public void Reset()
    {
        if (this.interfaceType == InterfaceType.Shop)
        {
            Container.Reset();
        }
        else
        {
            Container.Clear();
        }
    }
}

[System.Serializable]
//he calls this Inventory
public class SuperInventory
{
    public InventorySlotObject[] Slots = new InventorySlotObject[24];

    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }

    public void Reset()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].ResetToDefault();
        }
    }
}

public delegate void SlotUpdated(InventorySlotObject slot, bool ignoreEquip);

[System.Serializable]
public class InventorySlotObject
{
    public ItemType AllowedItems;
    public bool locked;
    public float price;
    public ItemData DefaultItem;
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdated;
    public ItemData item;
    public ItemObject ItemObject
    {
        get
        {
            if (item != null && !string.IsNullOrEmpty(item.itemID))
            {
                return parent.inventory.database.GetItemByID(item.itemID);
            }
            return null;
        }
    }

    public InventorySlotObject()
    {
        item = null; // empty slot
    }
    public InventorySlotObject(ItemData item)
    {
        UpdateSlot(item, true);
    }
    public void UpdateSlot(ItemData item)
    {
 
        if(OnBeforeUpdated != null)
        {
            OnBeforeUpdated.Invoke(this, false);
        }
        this.item = item;
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this, false);
        }
    }

    public void UpdateSlot(ItemData item, bool sameInterface)
    {
        if (OnBeforeUpdated != null)
        {
            OnBeforeUpdated.Invoke(this , true);
        }
        this.item = item;
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this, true);
        }
    }

    public void UpdateSlot(ItemData item, bool locked = false, float price = 0)
    {

        if (OnBeforeUpdated != null)
        {
            OnBeforeUpdated.Invoke(this, false);
        }
        this.item = item;
        this.locked = locked;
        this.price = price;
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this, false);
        }
    }

    public void RemoveItem()
    {
        UpdateSlot(new ItemData(), true);
    }

    public void ResetToDefault()
    {
        UpdateSlot(DefaultItem, true, price);
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (AllowedItems == ItemType.All ||itemObject == null)
        {
            return true;
        }
       
        if (itemObject.type == AllowedItems){
            return true;
            }          
        
        return false;
    }
}