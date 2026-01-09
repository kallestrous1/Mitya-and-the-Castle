using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public AudioClip hitSoundEffect;
    public GameObject hitParticleEffect;
    public GameObject deathParticleEffect;

    public Animator ani;


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
            if(this.gameObject.tag == "Destructable")
            {
                if (ani)
                {
                    ani.SetTrigger("Shake");
                }
            }

            if (currentHealth <= 0)
            {
                TelemetryManager.instance.LogEvent("gameplay", $"{ this.name} destroyed") ;
                TelemetryManager.instance.LogEvent("destructable_destroyed", $"{ this.name} destroyed at position {transform.position}",
                    new EnemyDefeatedPayload
                    {
                        enemyType = this.name,
                        weaponUsed = FindFirstObjectByType<PlayerAttacks>().playerWeapon.activeWeapon.name,
                        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                        defeatPosition = transform.position
                    });
                GameObject deathEffect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
                deathEffect.transform.parent = null;
              
                if (this.transform.parent)
                {
                    if (this.transform.parent.GetComponent<StateChangingObject>())
                    {
                        this.transform.parent.GetComponent<StateChangingObject>().ChangeObjectState(false);
                    }
                    this.transform.parent.gameObject.SetActive(false);
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {
                    if (GetComponent<StateChangingObject>())
                    {
                        GetComponent<StateChangingObject>().ChangeObjectState(false);
                    }
                    this.transform.gameObject.SetActive(false);
                    Destroy(this.transform.gameObject);
                }
            }
        }
    }
}
