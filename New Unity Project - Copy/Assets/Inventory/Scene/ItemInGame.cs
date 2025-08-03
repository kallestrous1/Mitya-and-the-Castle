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
    public bool isActive;
    public string homeScene;

    private void Start()
    {
        this.id = item.data.Id;
    }

    public void LoadData(GameData data)
    {
        foreach (int ID in data.itemsToDestroy)
        {
            if(ID == this.id)
            {
                //Destroy(this.transform.parent.gameObject);
            }
        }
    }

    public void SaveData(GameData data)
    {
        if (!isActive)
        {
            data.itemsToDestroy.Add(id);
        }
    }
}


