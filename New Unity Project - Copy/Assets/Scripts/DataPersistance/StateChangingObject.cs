using UnityEngine;
using UnityEngine.SceneManagement;

public class StateChangingObject : MonoBehaviour, IDataPersistence
{
    public bool active;
    public bool newgameState;
    public string objectName = "boss bandit";


    public void ChangeObjectState(bool newState)
    {
        active = newState;
    }

    public void LoadData(GameData data)
    {
        
        if (data.stateChangingObjects.ContainsKey(objectName))
        {
            active = data.stateChangingObjects[objectName];
        }
        else
        {
            active = newgameState;
            data.stateChangingObjects.Add(objectName, active);
        }
        this.gameObject.SetActive(active);
    }

    public void SaveData(GameData data)
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

    public void ResetData(GameData data)
    {
        active = newgameState;

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
