using UnityEngine;

[CreateAssetMenu(fileName = "Old Heal Staff", menuName = "Inventory/Weapons/Old Heal Staff")]


public class OldHealStaff : WeaponObject
{
    public override void CastBaseWeaponSpell()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().changeHealth(5);
    }
}
