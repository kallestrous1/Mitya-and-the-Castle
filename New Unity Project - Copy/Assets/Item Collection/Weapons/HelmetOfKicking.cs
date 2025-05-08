using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetOfKicking", menuName = "Inventory/Charms/HelmetOfKicking")]

public class HelmetOfKicking : ItemObject
{
    public override void EquipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttacks>().setWeapon(1);
    }

    public override void UnequipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttacks>().setWeapon(0);
    }
}
