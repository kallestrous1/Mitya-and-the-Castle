using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    Animator ani;
  
    public float attackTimeStopper;
    float attackTimer;
    
    void Start()
    {       
        ani = GetComponent<Animator>();      
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.X) && attackTimer <= 0)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                ani.SetTrigger("UpAttack");
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                ani.SetTrigger("DownAttack");
            }
            else
            {
                ani.SetTrigger("Attack");
            }
            attackTimer = attackTimeStopper;
        }
    }
}
