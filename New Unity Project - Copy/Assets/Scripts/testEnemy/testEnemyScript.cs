using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEnemyScript : MonoBehaviour
{

   
    GameObject player;
    public float speed;
    public float changeTime;
    public float range;
    public float health;
    public float actionSpeed;
    public float knockBackForce;
    private bool isGrounded;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    Animator ani;

    private void Start()
    {      
        player = GameObject.FindWithTag("Player");
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        CheckIfGrounded();
        ani.SetBool("Grounded", isGrounded);
    }

   
    void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D playerRB = other.gameObject.GetComponent<Rigidbody2D>();
        PlayerHealth controller = other.gameObject.GetComponent<PlayerHealth>();
        if (controller != null)
        {
            var knockBackDirection = playerRB.transform.position - this.transform.position;
            knockBackDirection.Normalize();
           // playerRB.AddForce(knockBackForce * knockBackDirection);
            controller.changeHealth(-1);
        }

    }
    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {           
            isGrounded = false;
        }
    }
    public void changeHealth(float change)
    {
        health += change;
        if (health <= 0)
        {
            Destroy(this.gameObject, 0.0f);
        }
    }
}