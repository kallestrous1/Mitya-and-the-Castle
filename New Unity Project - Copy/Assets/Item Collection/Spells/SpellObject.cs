using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponObject", menuName = "Inventory/Items/SpellObject")]

public class SpellObject : ItemObject
{
    public GameObject BaseParticleEffect;
    public float spellSpeed = 10f;
    public void Awake()
    {
        type = ItemType.Spell;
    }

    public override void EquipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInChildren<Animator>().runtimeAnimatorController = animations;  
    }

    public override void UnequipItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInChildren<Animator>().runtimeAnimatorController = baseAnimations;
       
    }

    public virtual void Cast(){ }
}
