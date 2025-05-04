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
        throw new System.NotImplementedException();
    }

    public override void UnequipItem()
    {
        throw new System.NotImplementedException();
    }
}
