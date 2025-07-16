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

    public bool flipped = false;
    public bool facingRight;

    public float TIMEBETWEENACTIONS = 3f;
    public float currentActionRecoveryTime = 0;


    // Start is called before the first frame update
    public virtual void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
     public virtual void Update()
    {
        if (currentActionRecoveryTime >= 0)
        {
            currentActionRecoveryTime -= Time.deltaTime;
        }

        /*if (target != null)
        {              
            facingRight = target.transform.position.x > this.transform.position.x;
            if (facingRight && !flipped)
            {
                flipped = true;
                this.transform.Rotate(0f, 180f, 0f);
            }
            else if(!facingRight && flipped)
            {
                flipped = false;
                this.transform.Rotate(0f, -180f, 0f);
            }

        }*/
    }

    public virtual void processHit() { }

    public virtual void EnemyBehaviour() { }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    public virtual bool checkIfReadyForAction() 
    {
        if (currentActionRecoveryTime <= 0)
        {
            currentActionRecoveryTime = TIMEBETWEENACTIONS;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void UpdateTurnDirection()
    {
        if (target != null)
        {
            facingRight = target.transform.position.x > this.transform.position.x;
            if (facingRight && !flipped)
            {
                flipped = true;
                // this.transform.Rotate(0f, 180f, 0f);
                float xScale = this.transform.localScale.x;
                this.transform.localScale = new Vector3(xScale * -1f, this.transform.localScale.y, this.transform.localScale.z);
            }
            else if (!facingRight && flipped)
            {
                flipped = false;
                float xScale = this.transform.localScale.x;
                this.transform.localScale = new Vector3(xScale * -1f, this.transform.localScale.y, this.transform.localScale.z);

                // this.transform.Rotate(0f, -180f, 0f);
            }

        }
    }
}
