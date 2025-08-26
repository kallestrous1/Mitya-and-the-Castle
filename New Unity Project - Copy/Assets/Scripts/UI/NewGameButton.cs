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

    IEnumerator SetActiveScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        DataPersistenceManager.instance.SaveGame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        NewManager.manager.unloadScene(4);

    }

}
