using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class ItemInGame : MonoBehaviour, IDataPersistence
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
        this.transform.parent.gameObject.SetActive(active);
        //this.id = item.data.Id;
        // Debug.Log(this.id);
        //this.id = 5;
    }

    public void ChangeState(bool state)
    {
        active = state;
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        if (data.itemStates.ContainsKey(itemName))
        {
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

    public void SaveData(GameData data)
    {
        if (this)
        {
            Debug.Log(active);
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

    public void ResetData(GameData data)
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


