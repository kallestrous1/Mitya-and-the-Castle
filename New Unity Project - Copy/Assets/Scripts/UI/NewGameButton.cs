using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



public class NewGameButton : BasicButton
{
    public AudioClip startGameSound;
    bool clicked = false;


    public void StartNewGame()
    {
        if (!clicked)
        {
            clicked = true;
            if (startGameSound)
            {
                AudioManager.Instance.Play(startGameSound);
            }

            NewManager.manager.NewGame();          
        }
    }
}
