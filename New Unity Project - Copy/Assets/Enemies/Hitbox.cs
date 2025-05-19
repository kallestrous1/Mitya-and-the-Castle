using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public string attackName;
    public int damage;
    public float knockback;

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            var force = trigger.transform.position - transform.position;
            force.Normalize();
            Rigidbody2D playerrb = trigger.GetComponent<Rigidbody2D>();
            trigger.GetComponent<PlayerHealth>().changeHealth(-damage);
            playerrb.AddForce(force * knockback, ForceMode2D.Impulse);
        }
    }
}
