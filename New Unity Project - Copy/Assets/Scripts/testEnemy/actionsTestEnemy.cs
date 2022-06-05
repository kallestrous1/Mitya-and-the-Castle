using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsTestEnemy : MonoBehaviour
{
    Rigidbody2D rb;
    Rigidbody2D playerRB;
    GameObject player;
    Animator ani;
    public float range;
    public float actionSpeed;
    bool awoken = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        playerRB = player.gameObject.GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }
    private void Update()
    {
        Vector2 position = rb.position;
        Vector2 playerPosition = playerRB.position;
        if (Vector2.Distance(position, playerPosition) < range&& (awoken==false))
        {
            awoken = true;
            InvokeRepeating("action", 0.0f, actionSpeed);
        }
        if (rb.velocity.x>1||rb.velocity.x<-1)
        {
            ani.SetBool("MovingX", true);
        }
        else
        {
            ani.SetBool("MovingX", false);
        }
        moveDirection();
        
    }
    void action()
    {
        getDirectionToPlayer();
        rb.velocity = new Vector2(0, 0);
        Vector2 position = rb.position;
        Vector2 playerPosition = playerRB.position;       
        int choice = Random.Range(0, 3);  
        
        if (getDistanceFromPlayer()<=2)
        {
            swordAttack();
        }
        else if (choice == 1)
        {
            jump();
        }
        else if (choice == 2)
        {
            rb.velocity = new Vector2(getDirectionToPlayer() * 5, 0);
        }
        else
        {
           
        }
        
    }

    void swordAttack()
    {
        ani.SetTrigger("Attack");
    }
    void jump()
    {
        rb.velocity = new Vector2(getDirectionToPlayer() * 5, 17);
    }


    float getDirectionToPlayer()
    {
        Vector2 position = rb.position;
        Vector2 playerPosition = playerRB.position;
        Vector2 relativePos = GameObject.FindWithTag("Player").transform.position - transform.position;
        if (relativePos.x < 0)
        {            
            return -1;
        }
        else if (relativePos.x > 0)
        {           
            return 1;
        }
        else
        {
            return 0;
        }
    }
    float getDistanceFromPlayer()
    {
        Vector2 position = rb.position;
        Vector2 playerPosition = playerRB.position;
        return Vector2.Distance(position, playerPosition);
    }
    void moveDirection()
    {
        if (rb.velocity.x > 0)
        {
            ani.SetBool("FacingRight", true);
        }
        else if (rb.velocity.x < 0)
        {
            ani.SetBool("FacingRight", false);
        }
    }

}
