using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    Rigidbody2D rb;
    Rigidbody2D playerrb;

    Collider2D weaponCollider;

    public WeaponObject activeWeapon;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D enemyrb = collision.GetComponent<Rigidbody2D>();

            if (activeWeapon.hitParticleEffect)
            {
                Instantiate(activeWeapon.hitParticleEffect, transform.position, Quaternion.identity);
            }
            collision.GetComponent<DamageFlash>().CallDamageFlash();
            collision.GetComponent<Destructable>().changeHealth(-activeWeapon.baseAttackDamage);
            collision.GetComponent<Enemy>().processHit();
            var force = (Vector2)collision.transform.position - (Vector2)this.gameObject.GetComponentInParent<Transform>().position;
            force.Normalize();
            enemyrb.AddForce(force * activeWeapon.weaponForce, ForceMode2D.Impulse);
        }
        else if(collision.gameObject.tag == "Destructable")
        {
            if (activeWeapon.hitParticleEffect)
            {
                Instantiate(activeWeapon.hitParticleEffect, transform.position, Quaternion.identity);
            }
            collision.GetComponent<Destructable>().changeHealth(-activeWeapon.baseAttackDamage);
        }
    }

    public void SetPlayerWeaponHitboxState(float stateNumber)
    {
        bool stateBool = false; 
        if(stateNumber == 1)
        {
            stateBool = true;
        }
        weaponCollider = GetComponent<Collider2D>();
        weaponCollider.enabled = stateBool;
    }

    public void castBaseActiveSpell()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMagicJuice>().currentMagic >= activeWeapon.baseSpellMagicCost)
        {
            if (activeWeapon.baseSpellParticle)
            {
                Instantiate(activeWeapon.baseSpellParticle, transform.position, Quaternion.identity);               
            }
            activeWeapon.CastBaseWeaponSpell();
            gameObject.GetComponentInParent<PlayerMagicJuice>().changeMagic(-activeWeapon.baseSpellMagicCost);
        }
    }
}
