using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour
{
    public ItemDetailsInterface ItemDetailsInterface;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public Dictionary<GameObject, InventorySlotObject> slotsOnInterface = new Dictionary<GameObject, InventorySlotObject>();

    public GameObject inGameItemPrefab;
    GameObject testItem;
    public Instantiator Instantiator;


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

    private void OnSlotUpdate(InventorySlotObject slot)
    {
        if (slot.item.Id >= 0)
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
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
        ItemDetailsInterface = GameObject.FindGameObjectWithTag("ItemDetailsInterface").GetComponent<ItemDetailsInterface>();
        if (slotsOnInterface[obj].item.Id >= 0)
            ItemDetailsInterface.setInterface(slotsOnInterface[obj].ItemObject);
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
            
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if(slotsOnInterface[obj].item.Id >= 0)
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
        Destroy(MouseData.tempItemBeingDragged);

        if(MouseData.interfaceMouseIsOver == null)
        {
            //problems will brew when you introduce identical items... items are stored in itemtracker via a dictionary (no duplicate keys)
            Instantiator.CreateItem(slotsOnInterface[obj].ItemObject);
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        if (MouseData.slotHoveredOver)
        {
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
            if (slot.Value.item.Id >= 0)
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
