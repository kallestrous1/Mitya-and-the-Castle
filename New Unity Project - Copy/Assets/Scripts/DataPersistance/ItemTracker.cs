using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemTracker : MonoBehaviour, IDataPersistence
{
    public List <ItemObject> collectedItems;
    public SerializableDictionary<ItemObject, Vector2> itemsInGame; //the fact that this is a dictionary doesn't allow for identical items
    public Instantiator instantiator;

    IEnumerator SpawnItems()
    {
        //0.1 seconds slower than the one that sets the correct active scene in startbutton and scenedoors, otherwise items will spawn in superscene
        yield return new WaitForSeconds(0.2f);
        instantiator = FindAnyObjectByType<Instantiator>();
        foreach (KeyValuePair<ItemObject, Vector2> item in itemsInGame)
        {
            if (item.Key.spawnScene == SceneManager.GetActiveScene().name)
            {
                Debug.Log("Item tracker spawning in: " + item.Key);
                instantiator.SpawnItem(item.Key, item.Value);
            }
        }
    }

    public void LoadData(GameData data)
    {
        this.collectedItems = data.collectedItems;
        this.itemsInGame = data.activeItems;
        StartCoroutine(SpawnItems());
    }

    public void SaveData(GameData data)
    {
        data.collectedItems = this.collectedItems;
        data.activeItems = this.itemsInGame;
    }

    public void ResetItems()
    {
        Debug.Log("reseting items");
        collectedItems.Clear();
        itemsInGame.Clear();
    }


}
