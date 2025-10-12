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
    public GameObject baseSpellParticle;

    public float weaponForce = 10;
    public int baseSpellMagicCost = 0;

    public void Awake()
    {
        type = ItemType.Weapon;                
    }


    public override void EquipItem()
    {
        Debug.Log("equipping item");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Animator>().runtimeAnimatorController = animations;

        PlayerWeaponContainer = GameObject.FindGameObjectWithTag("Weapon Container");
        PlayerWeaponContainer.GetComponentInChildren<SpriteRenderer>().sprite = weaponSprite;
        PlayerWeaponContainer.GetComponentInChildren<PlayerWeapon>().activeWeapon = this;
    }

    public override void UnequipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Animator>().runtimeAnimatorController = baseAnimations;
        PlayerWeaponContainer = GameObject.FindGameObjectWithTag("Weapon Container");
        PlayerWeaponContainer.GetComponentInChildren<SpriteRenderer>().sprite = null;
        PlayerWeaponContainer.GetComponentInChildren<PlayerWeapon>().activeWeapon = null;
    }

    public virtual void CastBaseWeaponSpell() { }
}
