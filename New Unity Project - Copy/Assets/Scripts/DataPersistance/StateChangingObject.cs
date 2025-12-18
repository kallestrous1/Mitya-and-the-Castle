using UnityEngine;
using UnityEngine.SceneManagement;

public class StateChangingObject : EntityPersistenceBehaviour         
{

    public bool active;
    public bool newgameState;

    public void ChangeObjectState(bool newState)
    {
        if (active == newState) return;

        active = newState;

        if (!DataPersistenceManager.instance.IsLoading)
        {
            DataPersistenceManager.instance.gameData.stateChangingObjects[DebugID] = active;
        }

        gameObject.SetActive(active);
    }

    public override void LoadData(GameData data)
    {

        if (data == null)
        {
            Debug.LogError($"{name}: GameData is null");
            return;
        }

        if (data.stateChangingObjects == null)
        {
            Debug.LogError($"{name}: stateChangingObjects dictionary is null");
            return;
        }

        if (data.stateChangingObjects.TryGetValue(DebugID, out bool savedState))
        {
            active = savedState;
        }
        else
        {
            active = newgameState;
            data.stateChangingObjects[DebugID] = active;
        }
        if (!this) return;              
        if (!gameObject) return;
        gameObject.SetActive(active);
    }

    public override void SaveData(GameData data)
    {
        if (data.stateChangingObjects.ContainsKey(DebugID))
            {
                data.stateChangingObjects[DebugID] = active;
            }
            else
            {
                data.stateChangingObjects.Add(DebugID, active);
            }
        
    }

    public override void ResetData(GameData data)
    {
        active = newgameState;

        if (data.stateChangingObjects.ContainsKey(DebugID))
            {
                data.stateChangingObjects[DebugID] = active;
            }
            else
            {
                data.stateChangingObjects.Add(DebugID, active);
            }    
    }


}
