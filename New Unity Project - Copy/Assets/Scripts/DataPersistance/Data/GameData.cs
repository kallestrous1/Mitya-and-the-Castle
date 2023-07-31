using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int totalJumps;

    public SerializableDictionary<Vector2, ItemObject> items;

    //default values:

    public GameData()
    {
        this.totalJumps = 0; 
    }

}
