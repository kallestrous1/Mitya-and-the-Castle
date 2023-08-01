using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemTracker : MonoBehaviour, IDataPersistence
{
    public SerializableDictionary<ItemObject, Vector2> itemsInGame;
    public Instantiator instantiator;

    IEnumerator SpawnItems()
    {
        //0.1 seconds slower than the one that sets the correct active scene in startbutton and scenedoors, otherwise items will spawn in superscene
        yield return new WaitForSeconds(0.2f);
        instantiator = FindObjectOfType<Instantiator>();
        foreach (KeyValuePair<ItemObject, Vector2> item in itemsInGame)
        {
            if (item.Key.spawnScene == SceneManager.GetActiveScene().name)
            {
                instantiator.SpawnItem(item.Key, item.Value);
            }
        }
    }

    public void LoadData(GameData data)
    {
        this.itemsInGame = data.activeItems;
        StartCoroutine(SpawnItems());
    }

    public void SaveData(ref GameData data)
    {
        data.activeItems = this.itemsInGame;
    }


}
