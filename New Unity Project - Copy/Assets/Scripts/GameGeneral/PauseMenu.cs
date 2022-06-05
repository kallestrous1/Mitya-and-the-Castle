using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;
    public PlayerHealth playerHealth;
    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }
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
        FindObjectOfType<AudioManager>().StopPlayingAudio("Rain");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        gamePaused = false;
    }
    void Pause()
    {
        FindObjectOfType<AudioManager>().PlayAudio("Rain");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        gamePaused = true;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("SuperScene");
        gamePaused = false;
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SaveGame()
    {
        SaveSystem.savePlayer(playerHealth);
        Debug.Log("Saved!");
    }
}
