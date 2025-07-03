using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public Slider maxHealthSlider;
    private RectTransform maxHealthRectTransform;
    private Vector2 maxHealthSliderInitialSize;

    public static int maxHealth=20;
    public int health=20;
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
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        maxHealthRectTransform = maxHealthSlider.GetComponent<RectTransform>();
        maxHealthSliderInitialSize = maxHealthRectTransform.sizeDelta;
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

    public void changeMaxHealth(int change)
    {
        maxHealth += change;
        maxHealthSlider.maxValue = maxHealth;
        maxHealthSlider.value = maxHealth;
        UpdateSliderSize(maxHealth);
    }

    public void changeHealth(int change, Vector2 knockBackDirection)
    {

         //   invincibilityTimer = invincibilityTime;
            health += change;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthSlider.value = health;
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

    public void UpdateSliderSize(float newMaxValue)
    {
        // Determine the scaling factor based on the new max value
        float scaleFactor = newMaxValue / 20;

        // Calculate the new size of the slider
        Vector2 newSize = new Vector2(maxHealthSliderInitialSize.x * scaleFactor, maxHealthSliderInitialSize.y);

        // Apply the new size to the slider's RectTransform
        maxHealthRectTransform.sizeDelta = newSize;
    }
}
