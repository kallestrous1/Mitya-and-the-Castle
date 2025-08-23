using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        StartCoroutine(LoadInventoryOnStartup());
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += Save;
    }

    private void OnDisable()
    {
        Save(new Scene());
        SceneManager.sceneUnloaded -= Save;

    }

    IEnumerator LoadInventoryOnStartup()
    {
        yield return new WaitForSeconds(1f);
        inventory.Load();
        equipment.Load();
        
    }

    public void OnAddItem(InventorySlotObject slot, bool ignoreEquip)
    {

        if (slot.ItemObject == null)
        {
            return;
        }
        if (ignoreEquip)
        {
            return;
        }
        switch (slot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                Debug.Log("Unequipping:  "+ slot.ItemObject);
                slot.ItemObject.UnequipItem();
                break;
            default:
                break;
        }

    }

    public void OnRemoveItem(InventorySlotObject slot, bool ignoreEquip)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        if (ignoreEquip)
        {
            return;
        }

        switch (slot.parent.inventory.interfaceType)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                Debug.Log("Equipping:  " + slot.ItemObject);
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

       /* if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
            equipment.Load();
        }*/
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
                    item.ChangeState(false); 
                    Destroy(collision.transform.parent.gameObject);
                }
            }
        }
    }

    public void Save(Scene scene)
    {
        inventory.Save();
        equipment.Save();
    }

    private void OnApplicationQuit()
    {
        inventory.Save();
        equipment.Save();

    }

}
