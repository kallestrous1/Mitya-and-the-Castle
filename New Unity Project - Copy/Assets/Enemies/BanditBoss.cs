using UnityEngine;
using System.Collections;


public class BanditBoss : Boss
{
    public int jumpForce;
    public int lateralJumpForce;
    public ParticleSystem shockWave;
    public ParticleSystem stompWave;

    public Transform shockWaveSpawn;

    public AudioClip swordToGroundSound;
    public AudioClip smashGroundSound;
    public AudioClip footStep;
    public AudioClip swordSwing;
    public AudioClip[] laughs;
    public AudioClip evilChant;



    public void PlayEvilChant()
    {
        AudioManager.Instance.Play(evilChant);

    }

    public void playLaughSound()
    {
        AudioManager.Instance.Play(laughs[Random.Range(0, laughs.Length)]);
    }

    public void playSwordSwing()
    {
        if (swordSwing)
        {
            AudioManager.Instance.Play(swordSwing);
        }
    }

    public void playFootStep()
    {
        if (footStep)
        {
            AudioManager.Instance.Play(footStep);
        }
    }

    public void PlaySwordToGroundSound()
    {
        if (swordToGroundSound)
        {
            AudioManager.Instance.Play(swordToGroundSound);
        }
    }

    public void playSmashGroundSound()
    {
        if (smashGroundSound)
        {
            AudioManager.Instance.Play(smashGroundSound);

        }
    }

    public void Jump()
    {
        StartCoroutine(ApplyJumpForce());
    }

    public void Stomp()
    {
        playSmashGroundSound();
        Instantiate(stompWave, shockWaveSpawn.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        EndFight();
    }
    private IEnumerator ApplyJumpForce()
    {
        facingRight = target.transform.position.x > this.transform.position.x;
        if (!facingRight)
        {
            rb.AddForce(new Vector2(-lateralJumpForce, jumpForce), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(lateralJumpForce, jumpForce), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.8f);
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        rb.AddForce(new Vector2(0, -jumpForce*3), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.25f);
        playSmashGroundSound();
        Instantiate(shockWave, shockWaveSpawn.position, Quaternion.identity);
    }
}
