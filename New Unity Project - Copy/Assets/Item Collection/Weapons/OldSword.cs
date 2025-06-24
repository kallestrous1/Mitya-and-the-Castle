using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Old Sword", menuName = "Inventory/Weapons/Old Sword")]

public class OldSword : WeaponObject
{
    public override void EquipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
       player.GetComponent<Animator>().runtimeAnimatorController = animations;
        GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<SpriteRenderer>().sprite = inGameSprite; 
    }

    public override void UnequipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Animator>().runtimeAnimatorController = baseAnimations;
        GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<SpriteRenderer>().sprite = null;
    }
}
