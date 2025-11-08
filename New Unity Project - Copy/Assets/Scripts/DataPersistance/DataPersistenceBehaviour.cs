using UnityEngine;

public abstract class DataPersistenceBehaviour : MonoBehaviour, IDataPersistence
{
    protected virtual void OnEnable()
    {
        DataPersistenceManager.Register(this);
    }

    protected virtual void OnDisable()
    {
        DataPersistenceManager.Unregister(this);
    }

    public abstract void LoadData(GameData data) ;
    public abstract void SaveData(GameData data);

    public virtual void ResetData(GameData data) { }
}
