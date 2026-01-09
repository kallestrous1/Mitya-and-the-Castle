using UnityEngine;

[CreateAssetMenu(fileName = "OldHealthCharm", menuName = "Inventory/Charms/OldHealthCharm")]

public class OldHealthCharm : CharmObject
{
    public override void EquipItem()
    {
        base.EquipItem();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().changeMaxHealth(5);
    }

    public override void UnequipItem()
    {
        base.UnequipItem();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().changeMaxHealth(-5);

    }
}
