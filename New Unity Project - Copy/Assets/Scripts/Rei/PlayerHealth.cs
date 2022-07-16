using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int maxHealth=5;
    public int health=5;
    public float invincibilityTime=1;
    float invincibilityTimer;
    public bool recovering;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
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

    public void changeHealth(int change)
    {
       
        if (invincibilityTimer <= 0 || change > 0)
        {
            invincibilityTimer = invincibilityTime;
            health += change;
        }
    }

    public int getHealth()
    {
        return health;
    }
}
