using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    Rigidbody2D rb;
    Rigidbody2D playerrb;

    Collider2D collider;

    public WeaponObject activeWeapon;

    void Start()
    {
        collider = transform.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D enemyrb = collision.GetComponent<Rigidbody2D>();

            if (activeWeapon.hitParticleEffect)
            {
                Instantiate(activeWeapon.hitParticleEffect, transform.position, Quaternion.identity);
            }

            collision.GetComponent<Destructable>().changeHealth(-activeWeapon.baseAttackDamage);
            collision.GetComponent<mosquito>().Stun();
            collision.GetComponent<DamageFlash>().CallDamageFlash();
            var force = (Vector2)collision.transform.position - (Vector2)this.gameObject.GetComponentInParent<Transform>().position;
            force.Normalize();
            enemyrb.AddForce(force * activeWeapon.weaponForce, ForceMode2D.Impulse);
        }
    }

    public void SetPlayerWeaponHitboxState(float stateNumber)
    {
        bool stateBool = false; 
        if(stateNumber == 1)
        {
            stateBool = true;
        }
        collider.enabled = stateBool;
    }
}
