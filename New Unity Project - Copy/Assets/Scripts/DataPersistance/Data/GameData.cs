using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int totalJumps;
    public  Vector2 playerSaveLocation;
    public  string playerSpawnScene;
    public List<ItemObject> collectedItems;
    public SerializableDictionary<ItemObject, Vector2> activeItems;

    //default values:

    public GameData()
    {
        this.totalJumps = 0; 
    }

}
