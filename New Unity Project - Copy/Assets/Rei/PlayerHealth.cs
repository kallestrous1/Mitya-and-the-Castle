using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    public static int maxHealth=5;
    public int health=5;
   // public float invincibilityTime=1;
    float invincibilityTimer;
    public bool recovering;

    public GameObject bloodDisplay;

    public AudioClip playerDamageSound;

    private KnockBack knockback;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        knockback = this.gameObject.GetComponent<KnockBack>();
    }
    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            recovering = true;
        }
        else
        {
            recovering = false;
        }
    }

    public void changeHealth(int change, Vector2 knockBackDirection)
    {
        if (true)
        {
         //   invincibilityTimer = invincibilityTime;
            health += change;
            if (change < 0)
            {
                AudioManager.Instance.Play(playerDamageSound);
                StartCoroutine(DisplayBlood());
                if (!knockback.isBeingKnockedBack)
                {
                    knockback.StartPlayerKnockback(knockBackDirection, Vector2.up, 0f);
                }
            }
        }
    }

    IEnumerator DisplayBlood()
    {
        bloodDisplay.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bloodDisplay.SetActive(false);
    }

    public int getHealth()
    {
        return health;
    }
}
