using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpellObject", menuName = "Inventory/Items/SpellObject")]

public abstract class SpellObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Spell;
    }
}
