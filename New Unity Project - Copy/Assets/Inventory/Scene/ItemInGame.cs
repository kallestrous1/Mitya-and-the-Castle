using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemInGame : EntityPersistenceBehaviour
{

    public ItemTracker itemTracker;
    public int id;
    public ItemObject item;
    public bool active;
    public bool newgameState;

    public string homeScene;

    [ContextMenu("Validate Persistence IDs")]
    void ValidateIDs()
    {
        Debug.Assert(!string.IsNullOrEmpty(DebugID), name);
    }
    private void Start()
    {
        //this.id = item.data.Id;
        // Debug.Log(this.id);
        //this.id = 5;
    }

    private void ApplyState()
    {
        if (transform.parent)
            transform.parent.gameObject.SetActive(active);
    }

    private IEnumerator ApplyStateNextFrame()
    {
        yield return null;
        ApplyState();
    }

    public void ChangeState(bool state)
    {
        if (active == state) return;

        active = state;
        if (!DataPersistenceManager.instance.IsLoading)
        {
            DataPersistenceManager.instance.gameData.stateChangingObjects[DebugID] = active;
        }

        StartCoroutine(ApplyStateNextFrame());
        if (!DataPersistenceManager.instance.IsLoading)
        {
            EventManager.OnItemStateChanged?.Invoke(DebugID, state);
        }
    }

    public override void LoadData(GameData data)
    {
        active = data.itemStates.TryGetValue(DebugID, out bool saved)
       ? saved
       : newgameState;
        if (!this) return;
        if( this.transform.parent)
            this.transform.parent.gameObject.SetActive(active);
        /* foreach (int ID in data.itemsToDestroy)
         {
             if (ID == this.id)
             {
                 try
                 {
                     Debug.Log(ID);
                     Destroy(this.transform.parent.gameObject);
                 }
                 catch (MissingReferenceException)
                 {
                     Debug.Log("Missing reference excpetion trying to delete item");
                 }
             }
         }*/
    }

    public override void SaveData(GameData data)
    {

            if (data.itemStates.ContainsKey(DebugID))
            {
                data.itemStates[DebugID] = active;
            }
            else
            {
                data.itemStates.Add(DebugID, active);
            }
        

        /*     if (!isActive)
             {
                 data.itemsToDestroy.Add(id);
             }
         */

    }

    public override void ResetData(GameData data)
    {
        active = newgameState;
        if (this)
        {
            if (data.itemStates.ContainsKey(DebugID))
            {
                data.itemStates[DebugID] = active;
            }
            else
            {
                data.itemStates.Add(DebugID, active);
            }
        }
    }
}
   