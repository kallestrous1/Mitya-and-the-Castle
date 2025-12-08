using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private static bool gamePaused = false;
    private NewManager manager;

    private void Start()
    {
        manager = NewManager.manager;
        pauseMenuUI.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else if (GameStateManager.instance.gameState == GameState.Play)
            {
                Pause();
            }
            else if (GameStateManager.instance.gameState == GameState.inventory)
            {
                InventoryController.instance.ExitInventory();
            }
            else if (GameStateManager.instance.gameState == GameState.dialogue)
            {
                DialogueManager.instance.ForceExitDialogueMode();
            }
        }
    }

    public void Resume()
    {    
        pauseMenuUI.SetActive(false);
         Time.timeScale = 1.0f;
        GameStateManager.instance.ChangeState(GameState.Play);

        gamePaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameStateManager.instance.ChangeState(GameState.Paused);
        Time.timeScale = 0.0f;
        gamePaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1.0f;

        gamePaused = false;
        GameStateManager.instance.ChangeState(GameState.MainMenu);
        NewManager.manager.MoveToScene("Menu", SceneManager.GetActiveScene().name, false);
        if (SceneManager.GetSceneByName("Base Scene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Base Scene");
        }

    }
    public void QuitGame()
    {
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }
}
