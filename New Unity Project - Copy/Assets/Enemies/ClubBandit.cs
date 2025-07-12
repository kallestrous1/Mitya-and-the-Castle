using System.Collections;
using UnityEngine;

public class ClubBandit : Enemy
{
    public override void Update()
    {
        if (currentActionRecoveryTime >= 0)
        {
            currentActionRecoveryTime -= Time.deltaTime;
        }
    }

 

    public void Jump()
    {
        StartCoroutine(ApplyJumpForce());
    }

    private IEnumerator ApplyJumpForce()
    {
        facingRight = target.transform.position.x > this.transform.position.x;
        if (!facingRight)
        {
            rb.AddForce(new Vector2(-100, 250), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(100, 250), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.5f);
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        rb.AddForce(new Vector2(0, -250), ForceMode2D.Impulse);
    }

    public void Dodge()
    {
        StartCoroutine(DodgeForce());

    }

    private IEnumerator DodgeForce()
    {
        facingRight = target.transform.position.x > this.transform.position.x;
        if (!facingRight)
        {
            rb.AddForce(new Vector2(250, 50), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(-250, 50), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.35f);
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
    }
}
