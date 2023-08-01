using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;
    public override void interact()
    {
       //I don't think all this ever runs or does anything but I'm not sure completely
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
