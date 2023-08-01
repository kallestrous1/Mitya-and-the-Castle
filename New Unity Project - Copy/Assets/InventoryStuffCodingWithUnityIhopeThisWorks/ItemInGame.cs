using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInGame : MonoBehaviour
{
    public ItemObject item;
    public bool isActive = true;
    public string homeScene;

    public void Start()
    {
        if (isActive == false)
        {
            Destroy(transform.parent.gameObject);
        }
    }

}


