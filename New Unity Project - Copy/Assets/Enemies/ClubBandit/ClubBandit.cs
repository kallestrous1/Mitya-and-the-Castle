using System.Collections;
using UnityEngine;

public class ClubBandit : Enemy
{

    public AudioSource audioSource;
    public AudioClip clubHitGround;
    public AudioClip clubSwing;
    public AudioClip footStep;
    public AudioClip yell;

    AlertAreaScript alertArea;

    public override void Start()
    {
        base.Start();
        alertArea = GetComponentInChildren<AlertAreaScript>();
    }

    public override void Update()
    {
        if (currentActionRecoveryTime >= 0)
        {
            currentActionRecoveryTime -= Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        if (!PlayerIsInRange())
            StopCombat();
    }

    public override void OnPlayerLeftRange()
    {
        StopCombat();
    }

    public override bool IsPlayerInCombatRange()
    {
        return PlayerIsInRange();
    }

    void StopCombat()
    {
        StopAllCoroutines();
        if (audioSource && audioSource.isPlaying)
            audioSource.Stop();
        if (rb)
            rb.linearVelocity = Vector2.zero;
        if (!ani)
            return;

        ani.ResetTrigger("OverheadSwing");
        ani.ResetTrigger("JumpAttack");
        ani.ResetTrigger("Dodge");
        ani.SetBool("Run", false);
        ani.SetBool("Taunt", false);
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("clubbanditidle"))
            ani.Play("clubbanditidle");
    }

    bool PlayerIsInRange()
    {
        return alertArea != null && alertArea.playerDetected;
    }

    public void PlayYell()
    {
        if (!PlayerIsInRange() || !yell)
            return;
        audioSource.clip = yell;
        audioSource.Play();
    }

    public void playFootStep()
    {
        if (!PlayerIsInRange())
            return;
        if (footStep)
        {
            audioSource.clip = footStep;
            audioSource.Play();
        }
    }
    public void playClubSwing()
    {
        if (!PlayerIsInRange())
            return;
        if (clubSwing)
        {
            audioSource.clip = clubSwing;
            audioSource.Play();
        }
    }

    public void playClubHitGround()
    {
        if (!PlayerIsInRange())
            return;
        if (clubHitGround)
        {
            audioSource.clip = clubHitGround;
            audioSource.Play();
        }
    }

    public void Jump()
    {
        if (!PlayerIsInRange())
            return;
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
        if (!PlayerIsInRange())
            return;
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
