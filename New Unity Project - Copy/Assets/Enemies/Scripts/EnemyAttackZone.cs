using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{

    public HumanDude humanDude;

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            humanDude.tryPunch();
        }
    }
}
