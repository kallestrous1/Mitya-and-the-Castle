using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharmObject", menuName = "Inventory/Items/CharmObject")]
public abstract class CharmObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Charm;
    }

    public override void EquipItem()
    {
        base.EquipItem();
    }

    public override void UnequipItem()
    {
       base.UnequipItem();
    }
}
