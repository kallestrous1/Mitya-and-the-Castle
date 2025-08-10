using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public AudioClip quitSound;
    public AudioClip hoverSound;
    public void QuitGame()
    {
        if (quitSound)
        {
            AudioManager.Instance.Play(quitSound);
        }
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
 
    }
}


