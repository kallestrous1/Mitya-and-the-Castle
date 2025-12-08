using UnityEngine;
using UnityEngine.SceneManagement;

public class StateChangingObject : DataPersistenceBehaviour         
{
    public bool active;
    public bool newgameState;
    public string objectName = "boss bandit";


    public void ChangeObjectState(bool newState)
    {
        active = newState;
    }

    public override void LoadData(GameData data)
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
        Debug.Log("Loading state for " + objectName + ": " + active);
        this.gameObject.SetActive(active);
    }

    public override void SaveData(GameData data)
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

    public override void ResetData(GameData data)
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
