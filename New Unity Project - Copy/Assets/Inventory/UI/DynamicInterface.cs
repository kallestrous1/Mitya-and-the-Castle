using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMNS;
    public int Y_SPACE_BETWEEN_ITEMS;

    public GameObject inventoryPrefabAll;
    public GameObject inventoryPrefabWeapon;
    public GameObject inventoryPrefabSpell;
    public GameObject inventoryPrefabCharm;

    public GameObject itemBlockerPrefab;



    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlotObject>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            GameObject obj = null;
            if (inventory.GetSlots[i].AllowedItems == ItemType.All) { 
                obj = Instantiate(inventoryPrefabWeapon, Vector3.zero, Quaternion.identity, transform);
            }
            else if(inventory.GetSlots[i].AllowedItems == ItemType.Charm)
            {
                obj = Instantiate(inventoryPrefabCharm, Vector3.zero, Quaternion.identity, transform);
            }
            else if (inventory.GetSlots[i].AllowedItems == ItemType.Weapon)
            {
                obj = Instantiate(inventoryPrefabWeapon, Vector3.zero, Quaternion.identity, transform);
            }
            else if (inventory.GetSlots[i].AllowedItems == ItemType.Charm)
            {
                obj = Instantiate(inventoryPrefabSpell, Vector3.zero, Quaternion.identity, transform);
            }
            if (inventory.GetSlots[i].locked)
            {
                itemBlockerPrefab = Instantiate(itemBlockerPrefab, Vector2.zero, Quaternion.identity, obj.transform);
            }
                                       
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
           

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);

            inventory.GetSlots[i].parent = this;
            if (obj.GetComponentInChildren<TextMeshProUGUI>())
            {
                if (inventory.GetSlots[i].ItemObject)
                {
                    Debug.Log(inventory.GetSlots[i].ItemObject.price.ToString());
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetSlots[i].ItemObject.price.ToString();
                }
              
            }

            
        }

        inventory.Load();
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + ((-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS))), 0f);
    }
}
