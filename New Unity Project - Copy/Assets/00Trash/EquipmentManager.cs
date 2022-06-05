using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    Inventory inventory;
    Equipment[] currentEquipment;

    void Start()
    {
        inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public void Equip(Equipment newItem)
    {
        Debug.Log("equipping: " + newItem.name);
        inventory.currentBackPack.AddItem(newItem);


        //old method;
       /* Equipment oldItem = null;
        int slotIndex = (int)newItem.equipSlot;
        if (currentEquipment[slotIndex]!=null)
        {
      
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }
        currentEquipment[slotIndex] = newItem;*/
    }
    public void Unequip(int slotIndex)
    {
        inventory.currentBackPack.Unequip(slotIndex);

        //old method
       /* if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
            currentEquipment[slotIndex] = null;
        }*/
    }
}
