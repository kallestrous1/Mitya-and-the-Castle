using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
   
    public Image icon;
    public Item item=null;
    public InventorySlotType slotType;

    public InventorySlot(InventorySlotType slotType)
    {
        this.slotType = slotType;
    }
    public void addItem(Item newItem)
    {
        this.item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;    
    }
    public void clearSlot()
    {
        this.item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public Item getItem()
    {
        return item;
    }

    public void Use()
    {
        item.Use();
    }
    public enum InventorySlotType { charm, weapon }
}
