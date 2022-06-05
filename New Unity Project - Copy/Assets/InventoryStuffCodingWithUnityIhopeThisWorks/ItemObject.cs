using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Spell,
    Charm
}

public abstract class ItemObject : ScriptableObject
{
   // public ItemScript playerItem;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea (15,20)]
    public string description;
    public SuperItem data = new SuperItem();
    public abstract void EquipItem();
    public abstract void UnequipItem();

}

[System.Serializable]
//he calls this Item
public class SuperItem
{
    public int Id = -1;
    public string name;
    public SuperItem()
    {
        name = "";
        Id = -1;
    }
    public SuperItem(ItemObject item)
    {
        Id = item.data.Id;
        name = item.name;
    }
}
