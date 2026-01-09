using Ink.Runtime;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    All,
    Weapon,
    Spell,
    Charm
}

public abstract class ItemObject : ScriptableObject
{
    public string itemID; // GUID string

    [ContextMenu("Generate GUID")]
    private void GenerateGUID()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    [Header("Active / Scene")]
    public bool setActive = true;
    public string spawnScene;

    [Header("Visuals")]
    public Sprite uiDisplay;
    public GameObject characterDisplay;
    public RuntimeAnimatorController animations;
    public RuntimeAnimatorController baseAnimations;
    public Sprite inGameSprite;

    [Header("Story & Description")]
    [SerializeField] public TextAsset itemStory;
    [TextArea(15, 20)]
    public string description;

    [Header("Gameplay & Economy")]
    public ItemType type;
    public float price;

    [Header("Runtime Data")]
    public ItemData data;

    private void OnValidate()
    {
        if (data == null)
        {
            data = new ItemData();
        }
    }

    private void OnEnable()
    {
        if (data == null)
            data = new ItemData();
    }

    public virtual void EquipItem() 
    {
        TelemetryManager.instance.LogEvent("item_equip", "equipping: " + this.name);
    }
    public virtual void UnequipItem()
    {
        TelemetryManager.instance.LogEvent("item_equip", "unequipping: " + this.name);

    }

}

[System.Serializable]
public class ItemData
{
    public string itemID;
    public string name;
    public ItemData(){}
    public ItemData(ItemObject item)
    {
        itemID = item.itemID;
        name = item.name;
    }
}
