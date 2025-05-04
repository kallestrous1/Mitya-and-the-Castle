using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponObject", menuName = "Inventory/Items/WeaponObject")]
public abstract class WeaponObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Weapon;                
    }
}
