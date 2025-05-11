using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public PlayerAttacks playerAttacks;
    Rigidbody2D rb;
    Rigidbody2D playerrb;
    Collider2D collider;

    float recoilMagnitude=300;

    public GameObject bloodEffectTest; 

    void Start()
    {
        playerrb = gameObject.transform.root.GetComponent<Rigidbody2D>();
        rb = transform.GetComponent<Rigidbody2D>();
        collider = transform.GetComponent<Collider2D>();
        rb.useFullKinematicContacts = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            GameObject blood = Instantiate(bloodEffectTest, transform.position , Quaternion.identity);
            blood.GetComponent<ParticleSystem>().Play();

            var force = transform.position - collision.transform.position;
            force.Normalize();
            playerrb.AddForce(force * recoilMagnitude, ForceMode2D.Impulse);
        }
    }

    public void SetSwordState(float stateNumber)
    {
        bool stateBool = false; 
        if(stateNumber == 1)
        {
            stateBool = true;
        }
        collider.enabled = stateBool;
    }
}
