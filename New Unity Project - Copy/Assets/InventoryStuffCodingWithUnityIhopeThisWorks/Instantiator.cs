using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject inGameItemPrefab;
    public GameObject inGameSpellPrefab;
    public GameObject inGameWeaponPrefab;
    public GameObject inGameCharmPrefab;


    public void CreateItem(ItemObject itemObject)
    {
        GameObject item;
        switch(itemObject.type)
        {
            case ItemType.Weapon: item = Instantiate(inGameWeaponPrefab);
                break;
            case ItemType.Charm: item = Instantiate(inGameCharmPrefab);
                break;
            case ItemType.Spell: item = Instantiate(inGameSpellPrefab);
                break;
            default: item = Instantiate(inGameItemPrefab);
                break;

        }
        item.GetComponentInChildren<ItemInGame>().item = itemObject;
        item.GetComponent<Transform>().position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * Random.Range(250.0f, 500.0f));
        rb.AddForce(transform.right * Random.Range(-500.0f, 500.0f));
    }
}
