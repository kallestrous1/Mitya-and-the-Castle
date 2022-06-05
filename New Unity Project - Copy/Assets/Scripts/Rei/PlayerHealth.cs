using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int maxHealth=5;
    public int health=5;
    public float invincibilityTime;
    float invincibilityTimer;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData data = SaveSystem.loadPlayer();
        health=data.health;
       // health = maxHealth;
    }
    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    public void changeHealth(int change)
    {
       
        if (invincibilityTimer <= 0 || change > 0)
        {
            invincibilityTimer = invincibilityTime;
            health += change;
         //   Debug.Log(health);
        }
    }
    public int getHealth()
    {
        return health;
    }

}
