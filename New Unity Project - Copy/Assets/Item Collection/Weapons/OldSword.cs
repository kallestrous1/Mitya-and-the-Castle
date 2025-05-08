using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Old Sword", menuName = "Inventory/Charms/Old Sword")]

public class OldSword : ItemObject
{

    public Sprite sword;
    public override void EquipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerAttacks>().setWeapon(0);
        GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<SpriteRenderer>().sprite = sword; 
    }

    public override void UnequipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttacks>().setWeapon(1);
        GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<SpriteRenderer>().sprite = null;
    }
}
