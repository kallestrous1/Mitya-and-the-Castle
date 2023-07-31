using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInGame : MonoBehaviour
{
    public ItemObject item;
    public bool pickedUp = false;

    public void Start()
    {
        if (pickedUp == true)
        {
            Destroy(transform.parent.gameObject);
        }
    }

}


