using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponObject", menuName = "Inventory/Items/WeaponObject")]
public abstract class WeaponObject : ItemObject
{

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
}
