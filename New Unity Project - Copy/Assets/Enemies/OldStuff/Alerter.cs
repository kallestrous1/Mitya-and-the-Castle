using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alerter : MonoBehaviour
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
        if(collider.tag == "Player")
        {
            mover.alert = true;
        }
    }
}
