using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;
    public override void interact()
    {
        base.interact();
        Inventory.instance.Add(item);
        Debug.Log("picking up" + item.name);
        PickUp();
        Destroy(gameObject);
    }

    void PickUp()
    {

    }
}
