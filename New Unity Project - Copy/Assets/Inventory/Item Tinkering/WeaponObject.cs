using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponObject", menuName = "Inventory/Items/WeaponObject")]
public abstract class WeaponObject : ItemObject
{
    public GameObject swordForPlayer;
    public GameObject PlayerWeaponContainer;
    public Sprite weaponSprite;

    public int baseAttackDamage;
    public int upAttackDamage;
    public int downAttackDamage;

    public AudioClip swingSound;

    public GameObject hitParticleEffect;

    public float weaponForce = 10;


    public void Awake()
    {
        type = ItemType.Weapon;                
    }


    public override void EquipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Animator>().runtimeAnimatorController = animations;

        PlayerWeaponContainer = GameObject.FindGameObjectWithTag("Weapon Container");
        PlayerWeaponContainer.GetComponentInChildren<SpriteRenderer>().sprite = weaponSprite;

    }

    public override void UnequipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Animator>().runtimeAnimatorController = baseAnimations;
        PlayerWeaponContainer.GetComponentInChildren<SpriteRenderer>().sprite = null;

    }
}
