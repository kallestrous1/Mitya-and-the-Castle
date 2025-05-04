using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.LoadScene("Base Scene", LoadSceneMode.Additive);
        StartCoroutine(SetActiveScene(sceneName));
    }

    IEnumerator SetActiveScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        DataPersistenceManager.instance.SaveGame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        NewManager.manager.unloadScene(3);

    }

}
