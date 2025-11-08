using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameButton : BasicButton
{
    public override void OnClicked()
    {
        base.OnClicked();
        NewManager.manager.currentGameState = GameState.Play;
        NewManager.manager.MoveToScene("Menu", SceneManager.GetActiveScene().name, false);
        SceneManager.UnloadSceneAsync("Base Scene");
        this.transform.parent.gameObject.SetActive(false);
    }
}
