using UnityEngine;

public interface IEntityPersistence : IDataPersistence
{
    string DebugID { get; }
}
