using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilGuyMovement : MonoBehaviour
{
    Animator animator;
    EnemyHealth enemyHealth;
    public Rigidbody2D rb;
    Transform player;
    public float sightRange = 7.0f;
    public float attackRange = 3.0f;
    public float actionSpeed = 0.1f;
    bool alert = false;
    LookAtPlayer lookAtPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        lookAtPlayer = animator.GetComponent<LookAtPlayer>();
        animator.SetFloat("Speed", 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(player.position, rb.position) <= sightRange&& alert == false)
        {
            alert = true;
            InvokeRepeating("action", 0.0f, actionSpeed);
        }      
        if(alert== true)
        {
            lookAtPlayer.lookAtPlayer();                      
        }
    }
//used to be knockback i think
   /* private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "PlayerWeapon")
        {
            Rigidbody2D playerRB = col.gameObject.GetComponentInParent<Rigidbody2D>();
            enemyHealth.changeHealth(-1);
            var knockBackDirection = playerRB.transform.position - col.transform.position;
            knockBackDirection.Normalize();
         //   playerRB.AddForce(5000 * knockBackDirection);
         //   rb.AddForce(5000 * -knockBackDirection);
        }
    }*/
    public void action()
    {
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 1);
        }
    }
  
}
