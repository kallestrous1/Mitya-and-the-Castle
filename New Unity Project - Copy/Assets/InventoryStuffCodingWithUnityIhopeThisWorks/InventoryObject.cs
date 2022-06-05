using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

public enum InterfaceType
{
    Inventory,
    Equipment
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]

public class InventoryObject : ScriptableObject
{
    public InterfaceType interfaceType;
    public string savePath;
    public ItemDataBaseObject database;
    public SuperInventory Container;
    public InventorySlotObject[] GetSlots{ get {return Container.Slots;}}

    public bool AddItem(SuperItem item)
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
                if(GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    //this is to find if the same item is already in the inventory, useful for stacking or something, but I don't need it rn
    public InventorySlotObject FindItemOnInventory(SuperItem item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.Id == item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }
    public InventorySlotObject setEmptySlot(SuperItem item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(item);
                return GetSlots[i];
            }
        }
        // what if inventory is full?
        return null;
    }

    public void SwapItems(InventorySlotObject itemOne, InventorySlotObject itemTwo)
    {
        if (itemTwo.CanPlaceInSlot(itemOne.ItemObject) && itemOne.CanPlaceInSlot(itemTwo.ItemObject)) 
        {
            InventorySlotObject temp = new InventorySlotObject(itemTwo.item);
            itemTwo.UpdateSlot(itemOne.item);
            itemOne.UpdateSlot(temp.item);

        }
    }

    public void RemoveItem(SuperItem item)
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
        //string saveData = JsonUtility.ToJson(Container, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            SuperInventory newContainer = (SuperInventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item);
            }
            stream.Close();
        }

    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
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
}

public delegate void SlotUpdated(InventorySlotObject slot);

[System.Serializable]
public class InventorySlotObject
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdated;
    public SuperItem item;

    public ItemObject ItemObject
    {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventory.database.GetItem[item.Id];
            }
            return null;
        }
    }

    public InventorySlotObject()
    {
        UpdateSlot(new SuperItem());
    }
    public InventorySlotObject(SuperItem item)
    {
        UpdateSlot(item);
    }
    public void UpdateSlot(SuperItem item)
    {
        if(OnBeforeUpdated != null)
        {
            OnBeforeUpdated.Invoke(this);
        }
        this.item = item;
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    public void RemoveItem()
    {
        UpdateSlot(new SuperItem());
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (AllowedItems.Length <= 0 ||itemObject == null || itemObject.data.Id < 0)
        {
            return true;
        }
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (itemObject.type == AllowedItems[i]){
                return true;
            }          
        }
        return false;
    }
}