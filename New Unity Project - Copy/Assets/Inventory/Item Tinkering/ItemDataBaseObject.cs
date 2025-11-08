using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Items/Database")]

public class ItemDataBaseObject : ScriptableObject
{
    public ItemObject[] ItemObjects;
    public Dictionary<string, ItemObject> GetItem = new Dictionary<string, ItemObject>();

#if UNITY_EDITOR
    [ContextMenu("Generate GUIDs")]
    private void GenerateGUIDs()
    {
        foreach (var item in ItemObjects)
        {
            if (item != null && string.IsNullOrEmpty(item.itemID))
            {
                item.itemID = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(item); // mark it dirty so it saves
            }
        }
    }
#endif

    public ItemObject GetItemByID(string id)
    {
        if (GetItem.Count == 0) PopulateDictionary();
        if (GetItem.TryGetValue(id, out var obj))
            return obj;
        Debug.LogWarning($"Item with ID {id} not found in database.");
        return null;
    }

    private void PopulateDictionary()
    {
        GetItem.Clear();
        foreach (var item in ItemObjects)
        {
            if (item == null || string.IsNullOrEmpty(item.itemID))
            {
                Debug.LogError("Item missing GUID: " + item?.name);
                continue;
            }
            GetItem[item.itemID] = item;
        }
    }
}
