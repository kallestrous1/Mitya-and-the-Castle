using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewManager : MonoBehaviour
{
    static bool gameStart;

    public static NewManager manager;

    private void Start()
    {
        if (!gameStart)
        {
            manager = this;
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
            gameStart = true;
        }
        else
        {
            manager = this;
            //no clue if this should be commented but it solved a weird error I just got
            //SceneManager.LoadScene(3, LoadSceneMode.Additive);
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("SaveReset")) // save reset set to 'k' for now
        {
            ItemTracker itemTracker = FindObjectOfType<ItemTracker>();
            itemTracker.ResetItems();
        }
    }

    public void unloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }
    IEnumerator Unload(int scene)
    {
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
    }

}
