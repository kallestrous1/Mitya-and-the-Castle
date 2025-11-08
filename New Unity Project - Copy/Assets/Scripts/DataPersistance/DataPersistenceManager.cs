using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Configuration")]

    [SerializeField] private string fileName = "saveData.json";

    public static DataPersistenceManager instance { get; private set; }

 
    private FileDataHandler dataHandler;
    private List<IDataPersistence> dataPersistanceObjects;
    private readonly List<IDataPersistence> registeredObjects = new List<IDataPersistence>();


    public GameData gameData;
    private bool saveRequested = false;
    public bool isNewGame = false;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Creating a second data persistence manager (not good)");
            Destroy(gameObject);
            return;
        }       
         
        instance = this;

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.OnItemStateChanged += UpdateItemState;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.OnItemStateChanged -= UpdateItemState;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(LoadAfterSceneLoaded());
    }

    private IEnumerator LoadAfterSceneLoaded()
    {
        // Wait one frame so all IDataPersistence objects register
        yield return null;

        LoadGame();
    }

    public void NewGame()
    {
        Debug.Log("Starting new game");
        isNewGame = true;
        this.gameData = new GameData();
        FindAllDataPersistenceObjects();
        foreach (var obj in registeredObjects)
            obj.ResetData(gameData);
        SaveGame();
        isNewGame = false;
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        Debug.Log("loading game");

        if (gameData == null)
        {
            Debug.Log("No data found, creating new game");
            NewGame();
            return;
        }

        foreach (var obj in registeredObjects)
            obj.LoadData(gameData);

    }

    public void SaveGame()
    {
        Debug.Log("Saving game");
        foreach (var obj in registeredObjects)
            obj.SaveData(gameData);
        dataHandler.Save(gameData);
    }
    public static void Register(IDataPersistence obj)
    {
        if (instance == null) return;

        if (!instance.registeredObjects.Contains(obj))
            instance.registeredObjects.Add(obj);
    }

    public static void Unregister(IDataPersistence obj)
    {
        if (instance == null) return;

        instance.registeredObjects.Remove(obj);
    }

    private void LateUpdate()
    {
        if (saveRequested)
        {
            SaveGame();
            saveRequested = false;
        }
    }
    private void UpdateItemState(string itemID, bool state)
    {
        gameData.itemStates[itemID] = state;
        saveRequested = true;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
     }


    private void OnApplicationQuit() =>  SaveGame();

}
