using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public AudioClip hitSoundEffect;
    public GameObject hitParticleEffect;
    public GameObject deathParticleEffect;


    //death animation? death sound effect?
    //respawn?


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void changeHealth(int change)
    {
        currentHealth += change;
        if (change < 0)
        {
            if (hitParticleEffect)
            {
                Instantiate(hitParticleEffect, transform.position, Quaternion.identity);
            }
            if (hitSoundEffect)
            {
                AudioManager.Instance.Play(hitSoundEffect);
            }

            if (currentHealth <= 0)
            {
                GameObject deathEffect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
                deathEffect.transform.parent = null;
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
