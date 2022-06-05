using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region singleton
    public static Inventory instance;

    private void Awake()   
    {
        if (instance != null) {
            Debug.LogWarning("More than one instance of Inventory");
            return;
        }
      instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;

    public List<Item> items = new List<Item>();

    public BackPack currentBackPack;

    public void Add (Item item)
    {
        Debug.Log("adding to inventory: " + item.name);
        items.Add(item);
        if (OnItemChangedCallback != null)      
            OnItemChangedCallback.Invoke();
        
    }
    public void Remove(Item item)
    {
        Debug.Log("removing from invenory: " + item.name);
        items.Remove(item);
        if (OnItemChangedCallback != null)
            OnItemChangedCallback.Invoke();
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentBackPack.slots.Length; i++)
        {
            currentBackPack.Unequip(i);
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("UnequipAll"))
        {
            UnequipAll();
        }
    }
}
