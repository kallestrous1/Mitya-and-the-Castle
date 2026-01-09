using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour
{
    public InterfaceType interfaceType;
    public ItemDetailsInterface ItemDetailsInterface;
    public InventoryObject inventory;
    public InventoryObject generalPlayerInventory;
    public InventoryObject equipment;
    public InventoryObject playerInventory;
    public Dictionary<GameObject, InventorySlotObject> slotsOnInterface = new Dictionary<GameObject, InventorySlotObject>();

    public GameObject inGameItemPrefab;
    GameObject testItem;
    public Instantiator Instantiator;

    public AudioClip hoverOverSlotSound;
    public AudioClip dragSound;
    public AudioClip dropSound;
    public AudioClip buySound;
    public AudioClip failToBuySound;
    public AudioClip failToDropSound;


    // Start is called before the first frame update
    void Awake()
    {
        CreateSlots();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        Instantiator = GameObject.FindGameObjectWithTag("Instantiator").GetComponent<Instantiator>();
    }

    public void StartCreateSlots()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        CreateSlots();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        Instantiator = GameObject.FindGameObjectWithTag("Instantiator").GetComponent<Instantiator>();
    }

    private void OnSlotUpdate(InventorySlotObject slot, bool ignoreEquip)
    {
        if (!string.IsNullOrEmpty(slot.item.itemID))
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemObject.uiDisplay;
            //code for full slot background //
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
           // slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            //code for empty slot background // slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite.color = new Color(1, 1, 1, 0);
           // slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }


    public abstract void CreateSlots();


    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            Debug.Log(obj.name + " does not have event trigger component");
        }

        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        if (hoverOverSlotSound)
        {
            AudioManager.Instance.Play(hoverOverSlotSound);
        }
        MouseData.slotHoveredOver = obj;
        ItemDetailsInterface = GameObject.FindGameObjectWithTag("ItemDetailsInterface").GetComponent<ItemDetailsInterface>();

        if (!string.IsNullOrEmpty(slotsOnInterface[obj].item.itemID))
            ItemDetailsInterface.setInterface(slotsOnInterface[obj].ItemObject);
    }
    public void OnExit(GameObject obj)
    {
        if (!string.IsNullOrEmpty(slotsOnInterface[obj].item.itemID))
            ItemDetailsInterface.ResetInterface();
        MouseData.slotHoveredOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
        if (dragSound)
        {
            AudioManager.Instance.Play(dragSound);
        }
        MouseData.slotHoveredOver = obj;
        if (MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].locked)
        {
            return;
        }
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if(!string.IsNullOrEmpty(slotsOnInterface[obj].item.itemID))
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        if (dropSound)
        {
            AudioManager.Instance.Play(dropSound);
        }
        Destroy(MouseData.tempItemBeingDragged);
        if (slotsOnInterface[obj].locked)
        {
            return;
        }

        if (MouseData.interfaceMouseIsOver == null)
        {
            AudioManager.Instance.Play(failToDropSound);
        }
        if (MouseData.interfaceMouseIsOver == null)
        {            
            return;
        }
        
            // deleting the functionality to drop items for now it is tooo much work and unneccersary august 12 2025
        /*  if (MouseData.interfaceMouseIsOver == null)
          {
              //problems will brew when you introduce identical items... items are stored in itemtracker via a dictionary (no duplicate keys)
              Instantiator.CreateItem(slotsOnInterface[obj].ItemObject);
              slotsOnInterface[obj].RemoveItem();
              return;
          }*/

            if (MouseData.slotHoveredOver)
        {
            if (MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].locked){
                return;
            }
            InventorySlotObject mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }

    }
    public void OnDrag(GameObject obj)
    {
    
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnClick(GameObject obj)
    {
        if (MouseData.interfaceMouseIsOver.inventory.interfaceType == InterfaceType.Shop)
        {
            if (MouseData.interfaceMouseIsOver.slotsOnInterface[obj].locked)
            {
                TryToUnlockSlot(obj);
            }
        }
        else if(MouseData.interfaceMouseIsOver.inventory.interfaceType == InterfaceType.Equipment)
        {
            InventorySlotObject emptyInventorySlot = generalPlayerInventory.FindEmptySlot();
            if(emptyInventorySlot == null)
            {
                if (failToBuySound)
                {
                    AudioManager.Instance.Play(failToBuySound);
                }
                return;
            }
            inventory.SwapItems(slotsOnInterface[obj], emptyInventorySlot);

            if (dragSound)
            {
                AudioManager.Instance.Play(dragSound);
            }
        }
        else if(MouseData.interfaceMouseIsOver.inventory.interfaceType == InterfaceType.Inventory)
        {
            if(slotsOnInterface[obj].item.itemID == "")
            {
                return;
            }
            if (MouseData.interfaceMouseIsOver.slotsOnInterface[obj].item.itemID == "")
            {
                return;
            }
            if(MouseData.interfaceMouseIsOver.slotsOnInterface[obj].ItemObject == null)
            {
                return;
            }
            ItemType type = MouseData.interfaceMouseIsOver.slotsOnInterface[obj].ItemObject.type;
            InventorySlotObject emptyEquipmentSlot = equipment.FindEmptySlotOfType(type);
            if (emptyEquipmentSlot == null)
            {
                if (failToBuySound)
                {
                    AudioManager.Instance.Play(failToBuySound);
                }
                return;
            }
            else
            {
                inventory.SwapItems(slotsOnInterface[obj], emptyEquipmentSlot);
                if (dragSound)
                {
                    AudioManager.Instance.Play(dragSound);
                }
            }
        }
    }

    public void TryToUnlockSlot(GameObject obj)
    {

        if(FindAnyObjectByType<PlayerMoney>().playerMoney >= MouseData.interfaceMouseIsOver.slotsOnInterface[obj].ItemObject.price)
        {
            GameObject buyslot = MouseData.slotHoveredOver;
            if (buySound)
            {
                AudioManager.Instance.Play(buySound);
            }
            TelemetryManager.instance.LogEvent("Shop", "Bought item: " + MouseData.interfaceMouseIsOver.slotsOnInterface[obj].ItemObject.name,
                new ItemPurchasedPayload
                {
                    itemType = MouseData.interfaceMouseIsOver.slotsOnInterface[obj].ItemObject.itemID,              
                    cost = ((int)MouseData.interfaceMouseIsOver.slotsOnInterface[obj].ItemObject.price),
                    totalMoney = ((int)FindAnyObjectByType<PlayerMoney>().playerMoney)
                });
            FindAnyObjectByType<PlayerMoney>().ChangePlayerMoneyCount(-MouseData.interfaceMouseIsOver.slotsOnInterface[obj].ItemObject.price);
            playerInventory.AddItem(MouseData.interfaceMouseIsOver.slotsOnInterface[obj].item);
            MouseData.interfaceMouseIsOver.slotsOnInterface[obj].locked = false;
            MouseData.interfaceMouseIsOver.slotsOnInterface[obj].UpdateSlot(new ItemData());
            buyslot.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else
        {
            if (failToBuySound)
            {
                AudioManager.Instance.Play(failToBuySound);
            }
        }
    }

    public void UpdateAllSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlotObject> slot in slotsOnInterface)
        {
            if (!string.IsNullOrEmpty(slot.Value.item.itemID))
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiDisplay;
                slot.Value.UpdateSlot();
                //code for full slot background // slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite.color = new Color(1, 1, 1, 1);
            }
        }
    }

}
    public static class MouseData
    {
        public static UserInterface interfaceMouseIsOver;
        public static GameObject tempItemBeingDragged;
        public static GameObject slotHoveredOver;
    }

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlotObject> slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlotObject> slot in slotsOnInterface)
        {
            if (!string.IsNullOrEmpty(slot.Value.item.itemID))
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiDisplay;
                //code for full slot background // slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite.color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "1";
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                //code for empty slot background // slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite.color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}
