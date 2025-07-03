using UnityEngine;

[CreateAssetMenu(fileName = "OldHealthCharm", menuName = "Inventory/Charms/OldHealthCharm")]

public class OldHealthCharm : ItemObject
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
