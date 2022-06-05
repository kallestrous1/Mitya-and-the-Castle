using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneDoorScript : MonoBehaviour
{

    public string nextScene;
    public int previousScene=1;
    int moveBoost;
    bool loaded;
    bool unloaded;
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb.velocity.x > 0)
        {
            moveBoost = 1;
        }
        else
        {
            moveBoost = -1;
        }

        Vector2 prevpos = other.transform.position;
        other.transform.position = new Vector2(prevpos.x+moveBoost, prevpos.y);

        if (!loaded)
        {
            loaded = true;
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }
        if (!unloaded)
        {
            unloaded = true;
            NewManager.manager.unloadScene(previousScene);
        }
    }

}
