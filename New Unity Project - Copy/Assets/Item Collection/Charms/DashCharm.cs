using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashCharm", menuName = "Inventory/Charms/DashCharm")]

public class DashCharm : CharmObject
{
    public override void EquipItem()
    {
        base.EquipItem();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().DASHCOUNT += 1;
    }

    public override void UnequipItem()
    {
        base.UnequipItem();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().DASHCOUNT -= 1;
    }
}
