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
             GameObject blood = Instantiate(activeWeapon.hitParticleEffect, transform.position , Quaternion.identity);
             blood.GetComponent<ParticleSystem>().Play();

             collision.GetComponent<Destructable>().changeHealth(-activeWeapon.baseAttackDamage);

            var force = collision.transform.position - transform.position;
            force.Normalize();
            Rigidbody2D enemyrb = collision.GetComponentInParent<Rigidbody2D>();
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
