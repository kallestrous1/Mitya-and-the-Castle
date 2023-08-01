using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instantiator : MonoBehaviour
{
    public GameObject inGameItemPrefab;
    public GameObject inGameSpellPrefab;
    public GameObject inGameWeaponPrefab;
    public GameObject inGameCharmPrefab;

    public ItemTracker itemTracker; 

    //spawns at player with a random jump
    public void CreateItem(ItemObject itemObject)
    {
        GameObject item;

        switch (itemObject.type)
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
        ItemInGame itemInGame = item.GetComponentInChildren<ItemInGame>();

        itemInGame.homeScene = SceneManager.GetActiveScene().name;
        itemInGame.item = itemObject;
        itemInGame.isActive = itemObject.setActive;
        item.GetComponent<Transform>().position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * Random.Range(250.0f, 500.0f));
        rb.AddForce(transform.right * Random.Range(-500.0f, 500.0f));
        itemObject.spawnScene = itemInGame.homeScene;
        FindCurrentItemTracker().itemsInGame.Add(itemObject, item.GetComponent<Transform>().position);
    }

    //spawns in designated location without a jump
    public void SpawnItem(ItemObject itemObject, Vector2 itemLocation)
    {
        GameObject item;
        switch (itemObject.type)
        {
            case ItemType.Weapon:
                item = Instantiate(inGameWeaponPrefab);
                break;
            case ItemType.Charm:
                item = Instantiate(inGameCharmPrefab);
                break;
            case ItemType.Spell:
                item = Instantiate(inGameSpellPrefab);
                break;
            default:
                item = Instantiate(inGameItemPrefab);
                break;
        }

        item.GetComponentInChildren<ItemInGame>().item = itemObject;
        item.GetComponentInChildren<ItemInGame>().isActive = itemObject.setActive;
        item.GetComponent<Transform>().position = itemLocation;
    }

    public ItemTracker FindCurrentItemTracker()
    {
        this.itemTracker = GameObject.FindGameObjectWithTag("ItemTracker").GetComponent<ItemTracker>();

        return this.itemTracker;
    }
}
