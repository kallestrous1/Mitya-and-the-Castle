using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponObject", menuName = "Inventory/Items/SpellObject")]

public class SpellObject : ItemObject
{
    public GameObject BaseParticleEffect;
    public float spellSpeed = 10f;
    public float manaCost = 10f;
    public float chargeTime = 0.5f;

    public void Awake()
    {
        type = ItemType.Spell;
    }

    public override void EquipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInChildren<PlayerSpells>().currentSpell = this;
    }

    public override void UnequipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInChildren<PlayerSpells>().currentSpell = null;
    }

    public virtual void Cast(){ }
}
