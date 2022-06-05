using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiPivot : MonoBehaviour
{
    public Transform player;
   
    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.position;
    }
}
