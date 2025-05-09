using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneDoorVert : MonoBehaviour
{
    public Vector2 newPlayerPosition;
    public string nextScene;
    public int previousScene = 1;
    int moveBoost;
    bool loaded;
    bool unloaded;
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerRB.isKinematic = true;
        playerRB.velocity = new Vector2(0, 0);
        other.transform.position = newPlayerPosition;

        if (!loaded)
        {
            loaded = true;
           // NewManager.manager.moveScenes(nextScene, previousScene, upBoost);
            /*  SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
              StartCoroutine(SetActiveScene(nextScene));*/
        }
    }

    IEnumerator SetActiveScene(string sceneName)
    {
        DataPersistenceManager.instance.SaveGame();
        yield return new WaitForSeconds(0.1f);
        Debug.Log(sceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        if (!unloaded)
        {
            unloaded = true;
            NewManager.manager.unloadScene(previousScene);
        }
    }

}
