using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    public float health;
    public void changeHealth(float change)
    {
        health += change;
        if (health <= 0)
        {
            Destroy(this.gameObject, 0.0f);
        }
    }
}
