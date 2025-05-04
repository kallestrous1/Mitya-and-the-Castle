using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneDoorVert : MonoBehaviour
{

    public string nextScene;
    public int previousScene = 1;
    int moveBoost;
    bool loaded;
    bool unloaded;
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb.velocity.y > 0)
        {
            moveBoost = 10;
        }
        else
        {
            moveBoost = -5;
        }

        Vector2 prevpos = other.transform.position;
        other.transform.position = new Vector2(prevpos.x, prevpos.y+moveBoost);

        if (!loaded)
        {
            loaded = true;
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
            StartCoroutine(SetActiveScene(nextScene));
            //   SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
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
