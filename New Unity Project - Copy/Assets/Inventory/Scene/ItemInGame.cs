using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemInGame : DataPersistenceBehaviour
{
    public ItemTracker itemTracker;
    public int id;
    public ItemObject item;
    public bool active;
    public bool newgameState;

    public string homeScene;
    public string itemName;

    private void Start()
    {
        //this.id = item.data.Id;
        // Debug.Log(this.id);
        //this.id = 5;
    }

    public void ChangeState(bool state)
    {
        active = state;
        Debug.Log("Item " + itemName + " state changed to " + state);
        EventManager.OnItemStateChanged.Invoke(itemName, state);
    }

    public override void LoadData(GameData data)
    {
        if (data.itemStates.ContainsKey(itemName))
        {
            Debug.Log("Loading item state for " + itemName + ": " + data.itemStates[itemName]);
            active = data.itemStates[itemName];
        }
        this.transform.parent.gameObject.SetActive(active);
        /* foreach (int ID in data.itemsToDestroy)
         {
             if (ID == this.id)
             {
                 try
                 {
                     Debug.Log(ID);
                     Destroy(this.transform.parent.gameObject);
                 }
                 catch (MissingReferenceException)
                 {
                     Debug.Log("Missing reference excpetion trying to delete item");
                 }
             }
         }*/
    }

    public override void SaveData(GameData data)
    {
        if (this)
        {
            Debug.Log(itemName + "state saved as: " + active);
            if (data.itemStates.ContainsKey(itemName))
            {
                data.itemStates[itemName] = active;
            }
            else
            {
                data.itemStates.Add(itemName, active);
            }
        }

        /*     if (!isActive)
             {
                 data.itemsToDestroy.Add(id);
             }
         */

    }

    public override void ResetData(GameData data)
    {
        active = newgameState;
        if (this)
        {
            if (data.itemStates.ContainsKey(itemName))
            {
                data.itemStates[itemName] = active;
            }
            else
            {
                data.itemStates.Add(itemName, active);
            }
        }
    }
}