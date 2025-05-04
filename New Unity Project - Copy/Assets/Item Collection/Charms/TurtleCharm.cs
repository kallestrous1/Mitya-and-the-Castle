using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TurtleCharm", menuName = "Inventory/Charms/TurtleCharm")]

public class TurtleCharm : ItemObject
{    
    public override void EquipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().JUMPCOUNT += 1;
    }

    public override void UnequipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().JUMPCOUNT -= 1;
    }
}
