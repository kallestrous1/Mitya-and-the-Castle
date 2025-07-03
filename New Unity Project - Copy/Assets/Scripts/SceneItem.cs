using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour
{
    public ItemTracker itemTracker;
    public ItemObject item;
//this is for items tht live in a scene, rather than the ones spawned in by instantiator
//if the item is stored in the itemtracker delete the original right after spawning it in
//not ideal tbh
/*    public void Start()
    {
        itemTracker = FindObjectOfType<ItemTracker>();

        if (itemTracker.collectedItems.Contains(this.item))
        {
            Debug.Log("destroying " + item);
            Destroy(transform.parent.gameObject);
        }
    }*/
}
