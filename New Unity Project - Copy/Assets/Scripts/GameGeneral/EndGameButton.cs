using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameButton : BasicButton
{
    public override void OnClicked()
    {
        base.OnClicked();
        GameStateManager.instance.ChangeState(GameState.MainMenu);
        NewManager.manager.MoveToScene("Menu", SceneManager.GetActiveScene().name, false);
        SceneManager.UnloadSceneAsync("Base Scene");
        this.transform.parent.gameObject.SetActive(false);
    }
}
