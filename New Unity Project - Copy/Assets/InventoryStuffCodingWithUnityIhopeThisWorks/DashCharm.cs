using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashCharm", menuName = "Inventory/Charms/DashCharm")]

public class DashCharm : ItemObject
{
    public override void EquipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().DASHCOUNT += 1;
    }

    public override void UnequipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().DASHCOUNT -= 1;
    }
}
