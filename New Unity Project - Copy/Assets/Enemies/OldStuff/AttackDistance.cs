using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDistance : MonoBehaviour
{
    FlyMovement mover;
    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponentInParent<FlyMovement>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            mover.attackDistance = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            mover.attackDistance = false;
        }
    }
}
