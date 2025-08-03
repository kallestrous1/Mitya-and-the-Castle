using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {    
        pauseMenuUI.SetActive(false);
         Time.timeScale = 1.0f;
        NewManager.manager.currentGameState = GameState.Play;

        gamePaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        NewManager.manager.currentGameState = GameState.Paused;
        Time.timeScale = 0.0f;
        gamePaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1.0f;

        gamePaused = false;
        NewManager.manager.currentGameState = GameState.Play;
        NewManager.manager.moveScenes("Menu", SceneManager.GetActiveScene().buildIndex, false);
        SceneManager.UnloadSceneAsync("Base Scene");

    }
    public void QuitGame()
    {
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }
}
