using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    Transform target;
    public bool alert;
    public bool attackDistance;

    bool attacked;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            if(alert && !attackDistance)
            Move();
        else if(!attacked && alert)
        {
            rb.linearVelocity = Vector2.zero;
            Attack();
        }
    }

    void Move()
        {
        Vector2 dir = target.position - transform.position;
        dir = dir.normalized;
        rb.AddForce(dir * speed);
    }

    void Attack()
    {
        attacked = true;
        Vector2 dir = target.position - transform.position;
        dir = dir.normalized;
        rb.AddForce(dir * speed * 10, ForceMode2D.Impulse);
        rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
    }
}
