using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Play,
    Paused
}

public class NewManager : MonoBehaviour
{
    public GameState currentGameState;
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

    public void moveScenes(string newScene, int previousScene, bool upBoost)
    {
        currentGameState = GameState.Paused;
        unloadScene(previousScene);
        StartCoroutine(LoadingMenu(newScene, upBoost));
    }

    IEnumerator LoadingMenu(string newScene, bool upBoost)
    {
        SceneManager.LoadSceneAsync(10, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.5f);
        unloadScene(10);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic = false;
        if (upBoost)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Jump();
        }
        currentGameState = GameState.Play;
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
