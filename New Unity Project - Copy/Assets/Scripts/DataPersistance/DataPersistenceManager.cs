using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Configuration")]

    [SerializeField] private string fileName;

    private GameData gameData;

    private List<IDataPersistence> dataPersistanceObjects;

    private FileDataHandler dataHandler; 
    public static DataPersistenceManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Creating a second data persistence manager (not good)");        
        }       
         
        instance = this;

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data found, creating new game");
            NewGame();
        }

        foreach(IDataPersistence dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }

        Debug.Log("total jumps = " + gameData.totalJumps);
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(gameData);
        }

        Debug.Log("total jumps = " + gameData.totalJumps);

        dataHandler.Save(gameData);

    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
     }


    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
