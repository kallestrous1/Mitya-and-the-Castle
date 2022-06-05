using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject inGameItemPrefab;
    
    public void CreateItem(ItemObject itemObject)
    {
        Debug.Log(inGameItemPrefab);
        GameObject item;
        item = Instantiate(inGameItemPrefab);
        item.GetComponent<ItemInGame>().item = itemObject;
        item.GetComponent<Transform>().position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
    }
}
