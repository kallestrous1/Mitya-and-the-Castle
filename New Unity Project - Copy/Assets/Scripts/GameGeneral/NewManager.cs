using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Play,
    Paused
}

public class NewManager : MonoBehaviour, IDataPersistence
{
    public GameState currentGameState;
    static bool gameStart;

    public static Vector2 playerSaveLocation = new Vector2(2, -59);
    public static string playerSpawnScene = "Grandpa's Farm";

    public static NewManager manager;

    public GameObject loadingScreenPanel;
    public GameObject endGamePanel;

    public Slider loadingBar;

    public InventoryObject[] shops;

    private void Start()
    {
        if (!gameStart)
        {
            manager = this;
            SceneManager.LoadScene(4, LoadSceneMode.Additive);
            gameStart = true;
        }
        else
        {
            manager = this;
            //no clue if this should be commented but it solved a weird error I just got
            //SceneManager.LoadScene(3, LoadSceneMode.Additive);
        }

        
    }

    public void TriggerEndGame()
    {
        Debug.Log("the end!");
        endGamePanel.SetActive(true);
    }

    public void moveScenes(string newScene, string previousScene, bool upBoost)
    {
        currentGameState = GameState.Paused;
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(LoadingMenu(newScene, upBoost, false));
        unloadScene(previousScene);
    }

    public void addScene(string newScene, bool upBoost)
    {
        currentGameState = GameState.Paused;
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(LoadingMenu(newScene, upBoost, false));
    }

    public void NewGame()
    {
        DataPersistenceManager.instance.NewGame();
        //I do this in gamedata now
       // playerSpawnScene = "Grandpa's Farm";
        playerSaveLocation = new Vector2(2, -59);

        addScene("Base Scene", false);
        StartCoroutine(ResetStories());
        currentGameState = GameState.Paused;
        StartCoroutine(LoadingMenu("Grandpa's Farm", false, true));
        unloadScene("Menu");
        
    }

    IEnumerator ResetStories()
    {
        yield return new WaitForSeconds(10.0f);
        DialogueManager.instance.ResetAllStories();
    }

    IEnumerator LoadingMenu(string newScene, bool upBoost, bool newGame)
    {
        //SceneManager.LoadSceneAsync(10, LoadSceneMode.Additive);
        loadingScreenPanel.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        loadScene.allowSceneActivation = false;

        while (!loadScene.isDone)
        {
            float progress = Mathf.Clamp01(loadScene.progress); // Normalize progress to 0-1
            loadingBar.value = progress;
       
            if (loadScene.progress >= 0.9f)
            {
                // Optionally add a delay or fade-out effect here
                loadScene.allowSceneActivation = true;
            }

            yield return null; // Wait for next frame
        }

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }

        currentGameState = GameState.Play;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));

        if (upBoost)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Jump();
        }

        if (newGame)
        {
            DataPersistenceManager.instance.NewGame();
        }
        else
        {
            DataPersistenceManager.instance.LoadGame();
        }
        loadingScreenPanel.SetActive(false);

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if(newScene.Equals("Base Scene"))
            {
                yield return new WaitForSeconds(1f);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetToSavedLocation();             

            }
            if (newGame)
            {
                if (FindAnyObjectByType<ItemTracker>())
                {
                    FindAnyObjectByType<ItemTracker>().ResetItems();
                    Debug.Log("resetting items");
                }
                    
                if (FindAnyObjectByType<PlayerInventory>())
                {
                    PlayerInventory playerInventory = FindAnyObjectByType<PlayerInventory>();
                    playerInventory.inventory.Clear();
                    playerInventory.equipment.Clear();
                    playerInventory.inventory.Save();
                    playerInventory.equipment.Save();
                }
                foreach (InventoryObject shop in shops)
                {
                    shop.Reset();
                    shop.Save();
                }

            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        }
    }


    public void unloadScene(string scene)
    {
        StartCoroutine(Unload(scene));
    }
    IEnumerator Unload(string scene)
    {
        Debug.Log(scene);
        SceneManager.UnloadSceneAsync(scene);
        yield return null;
    }

    void IDataPersistence.LoadData(GameData data)
    {
        playerSaveLocation = data.playerSaveLocation;
        Debug.Log(data.playerSpawnScene);
        playerSpawnScene = data.playerSpawnScene;
    }

    void IDataPersistence.SaveData(GameData data)
    {
        data.playerSaveLocation = playerSaveLocation;
        data.playerSpawnScene = playerSpawnScene;
    }

    void IDataPersistence.ResetData(GameData data)
    {
        playerSaveLocation = new Vector2(2, -59);
        playerSpawnScene = "Grandpa's Farm";
        data.playerSaveLocation = playerSaveLocation;
        data.playerSpawnScene = playerSpawnScene;
    }

    private void OnApplicationQuit()
    {
        DataPersistenceManager.instance.SaveGame();
    }
}
