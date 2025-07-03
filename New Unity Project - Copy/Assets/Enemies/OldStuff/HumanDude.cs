using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDude : Enemy
{
    public float attackDistance;
    public float tauntDistance;

    float distance;

    public float RECOVERYTIME = 0.5f;
    public float recoveryTime = 0;

    public override void Update()
    {
        base.Update();

        if (recoveryTime > 0)
        {
            recoveryTime -= Time.deltaTime;
        }

        if(rb.linearVelocity.x > 1  && flipped == false)
        {
            this.transform.Rotate(0f, 180f, 0f);
            flipped = true;
            Vector2 currentPosition = this.transform.position;
            this.transform.position = new Vector2(currentPosition.x + 1, currentPosition.y);
        }
        else if( rb.linearVelocity.x < -1 && flipped == true)
        {
            this.transform.Rotate(0f, -180f, 0f);
            flipped = false;
            Vector2 currentPosition = this.transform.position;
            this.transform.position = new Vector2(currentPosition.x - 1, currentPosition.y);
        }
    }

    public override void EnemyBehaviour()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        if(distance > attackDistance  && distance >= tauntDistance)
        {
            ani.SetBool("Alert", false);
            Move();
        }
        else if(distance < tauntDistance && distance > attackDistance)
        {
            ani.SetBool("Alert", true);
            ApproachPlayer();
        }
        else if (distance <= attackDistance) {
            Attack();
        }
    }

    public void Attack()
    {
        if (recoveryTime <= 0)
        {
            ani.SetBool("Idle", false);
            ani.SetBool("Walking", false);
            ani.SetTrigger("Kick");
            recoveryTime = RECOVERYTIME;
        }
    }

    private void ApproachPlayer()
    {
        ani.SetBool("Idle", false);
        ani.SetBool("Walking", true);
        Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
        Vector2 direction = (targetPosition - rb.position).normalized;
        Vector2 force = direction * moveSpeed/4 * Time.deltaTime;
        rb.AddForce(force, ForceMode2D.Impulse);

    }

    public void Move()
    {
        Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
        Vector2 direction = (targetPosition - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;
        rb.AddForce(force, ForceMode2D.Impulse);

        ani.SetBool("Walking", true);
        ani.SetBool("Idle", false);

    }

    public void tryPunch()
    {
        if (recoveryTime <= 0)
        {
            ani.SetBool("Idle", false);
            ani.SetBool("Walking", false);
            ani.SetTrigger("Punch");
            recoveryTime = RECOVERYTIME;
        }
    }
}
