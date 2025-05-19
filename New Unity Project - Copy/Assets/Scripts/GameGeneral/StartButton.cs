using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    public string getSpawnScene()
    {
        return NewManager.playerSpawnScene;
    }

    public void LoadScene(string scene)
    {

        if (getSpawnScene() != null)
        {
            string spawnScene = getSpawnScene();
            SceneManager.LoadScene(spawnScene, LoadSceneMode.Additive);
            SceneManager.LoadScene("Base Scene", LoadSceneMode.Additive);
            StartCoroutine(SetActiveScene(spawnScene));

        }
        else
        {
            Debug.Log("Using default initial spawn scene");
            SceneManager.LoadScene("Grandpa's Farm", LoadSceneMode.Additive);
            SceneManager.LoadScene("Base Scene", LoadSceneMode.Additive);
            StartCoroutine(SetActiveScene("Grandpa's Farm"));
        }
        
    }

    IEnumerator SetActiveScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        DataPersistenceManager.instance.SaveGame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        NewManager.manager.unloadScene(3);

    }

}
