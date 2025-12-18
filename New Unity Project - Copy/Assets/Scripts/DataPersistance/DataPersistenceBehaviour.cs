using System.Xml;
using UnityEngine;

public abstract class DataPersistenceBehaviour : MonoBehaviour, IDataPersistence
{

    public virtual string DebugID => "(GLOBAL)";

    protected virtual void OnEnable()
    {
        Debug.Log("Registering " + this.GetType().Name + " to Data Persistence Manager.");
        DataPersistenceManager.Register(this);
    }

    protected virtual void OnDisable()
    {
        Debug.Log("Unregistering " + this.GetType().Name + " from Data Persistence Manager.");
        DataPersistenceManager.Unregister(this);
    }

    public abstract void LoadData(GameData data) ;
    public abstract void SaveData(GameData data);

    public virtual void ResetData(GameData data) { }
}
