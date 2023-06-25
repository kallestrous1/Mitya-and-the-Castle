using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    //CapsuleCollider2D bc;
    Rigidbody2D playerRB;
    public float knockBackForce;
    bool knocked;
    public float knockTime;
    float knockTimer;
    PlayerMovement playerMovement;
    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRB = GetComponentInParent<Rigidbody2D>();
        //  bc = gameObject.AddComponent<CapsuleCollider2D>() ;
    }
    private void Update()
    {
        if (knockTimer >= 0)
        {
            knockTimer -= Time.deltaTime;
        }
        else if (knockTimer <= 0)
        {
            knocked = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();
        testEnemyScript controller = other.gameObject.GetComponent<testEnemyScript>();
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (knocked == false)
            {
                knock();
                var knockBackDirection = other.transform.position - playerRB.transform.position;
                knockBackDirection.Normalize();
                otherRB.AddForce(knockBackDirection * 1000);
                playerMovement.addOutsideForce(-knockBackDirection*knockBackForce);
            }
            controller.changeHealth(-1);
        }
    }
    void knock()
    {
        knocked = true;
        knockTimer = knockTime;
    }
}
