using System.Collections;
using UnityEngine;

public class SpearBandit : Enemy
{
    

    public void Leap()
    {
        StartCoroutine(LeapForce());
    }

    private IEnumerator LeapForce()
    {
        facingRight = target.transform.position.x > this.transform.position.x;
        if (!facingRight)
        {
            rb.AddForce(new Vector2(-150, 0), ForceMode2D.Impulse);
        }
        else 
        {
            rb.AddForce(new Vector2(150, 0), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.2f);
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        rb.AddForce(new Vector2(0, -250), ForceMode2D.Impulse);
    }
}
