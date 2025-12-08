using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartButton : BasicButton, IPointerEnterHandler
{

    bool clicked = false;
    public AudioClip startGameSound;


    public string getSpawnScene()
    {
        return NewManager.manager.defaultPlayerScene;
    }

    public void LoadScene(string scene)
    {

        if (!clicked)
        {
            clicked = true;
            if (startGameSound)
            {
                AudioManager.Instance.Play(startGameSound);
            }
            if (getSpawnScene() != null)
            {             
                string spawnScene = getSpawnScene();
                // SceneManager.LoadScene(spawnScene, LoadSceneMode.Additive);
                NewManager.manager.AddScene("Base Scene", false);

                NewManager.manager.MoveToScene(spawnScene, "Menu", false);


                //    SceneManager.LoadSceneAsync("Base Scene", LoadSceneMode.Additive);
              //  StartCoroutine(SetActiveScene(spawnScene));

            }
            else
            {
                Debug.Log("Using default initial spawn scene");
                SceneManager.LoadScene("Grandpa's Farm", LoadSceneMode.Additive);
                SceneManager.LoadScene("Base Scene", LoadSceneMode.Additive);
             //   StartCoroutine(SetActiveScene("Grandpa's Farm"));
            }
        }
        
    }

    IEnumerator SetActiveScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("Setting active scene to " + sceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        NewManager.manager.UnloadScenePublic("Menu");

    }

}
