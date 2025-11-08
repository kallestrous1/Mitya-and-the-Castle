using UnityEngine;

[CreateAssetMenu(fileName = "OldHealthCharm", menuName = "Inventory/Charms/OldHealthCharm")]

public class OldHealthCharm : CharmObject
{
    public override void EquipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().changeMaxHealth(5);
    }

    public override void UnequipItem()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().changeMaxHealth(-5);

    }
}
