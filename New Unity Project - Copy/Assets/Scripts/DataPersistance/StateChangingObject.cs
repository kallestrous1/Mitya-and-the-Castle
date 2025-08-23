using UnityEngine;

public class StateChangingObject : MonoBehaviour, IDataPersistence
{
    public bool active;
    public bool newgameState;
    public string objectName = "boss bandit";



    void Start()
    {
        this.gameObject.SetActive(active);
    }

    void OnDisable()
    {
        active = false;
     //   DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        if (data.stateChangingObjects.ContainsKey(objectName))
        {
            active = data.stateChangingObjects[objectName];
        }       
        this.gameObject.SetActive(active);
    }

    public void SaveData(GameData data)
    {
        if (this)
        {
            if (data.stateChangingObjects.ContainsKey(objectName))
            {
                data.stateChangingObjects[objectName] = active;
            }
            else
            {
                data.stateChangingObjects.Add(objectName, active);
            }
        }
    }

    public void ResetData(GameData data)
    {
        active = newgameState;
        if (this)
        {
            if (data.stateChangingObjects.ContainsKey(objectName))
            {
                data.stateChangingObjects[objectName] = active;
            }
            else
            {
                data.stateChangingObjects.Add(objectName, active);
            }
        }
    }


}
