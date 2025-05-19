using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;

    public GameObject target;
    public Animator ani;
    public Rigidbody2D rb;
    public bool inRange;

    public int health;

    public bool flipped = false;



    // Start is called before the first frame update
    public virtual void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
     public virtual void Update()
    {
        if (inRange)
        {
            EnemyBehaviour();
        }
    }

    public virtual void EnemyBehaviour() { }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            target = trigger.gameObject;
            inRange = true;
        }
    }

    public virtual void doDamage(int damage) { }
}
