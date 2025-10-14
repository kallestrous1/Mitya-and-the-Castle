using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public string attackName;
    public int damage;
    public int stayDamage;
    public float knockback;
    public bool oneOff = false;

    public GameObject hitParticleEffect;

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Enemy")
        {
            trigger.GetComponent<Destructable>().changeHealth(-damage);
            var force = trigger.transform.position - transform.position;
            force.Normalize();
            Rigidbody2D enemyrb = trigger.GetComponentInParent<Rigidbody2D>();
            enemyrb.AddForce(force * knockback, ForceMode2D.Impulse);
            if (oneOff)
            {
                Destroy(this.transform.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Enemy")
        {
            trigger.GetComponent<Destructable>().changeHealth(-stayDamage);
            var force = trigger.transform.position - transform.position;
            force.Normalize();
            Rigidbody2D enemyrb = trigger.GetComponentInParent<Rigidbody2D>();
            enemyrb.AddForce(force * knockback, ForceMode2D.Impulse);

        }
    }

}
