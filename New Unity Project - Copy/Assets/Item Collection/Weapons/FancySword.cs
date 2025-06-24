using UnityEngine;

[CreateAssetMenu(fileName = "Fancy Sword", menuName = "Inventory/Weapons/Fancy Sword")]

public class FancySword : WeaponObject
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
