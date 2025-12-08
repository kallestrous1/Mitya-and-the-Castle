using UnityEngine;

public enum GameState
{
    MainMenu,
    Play,
    Paused,
    inventory,
    dialogue,
    movingScene,
}

public class GameStateManager : MonoBehaviour
{
    #region Singleton

       public static GameStateManager instance { get; private set; }

         private void Awake()
         {
              if (instance != null)
              {
                Destroy(gameObject);
              }
              else
              {
                instance = this;
                DontDestroyOnLoad(gameObject);
              }
    }

    #endregion


    public GameState gameState = GameState.MainMenu;

    public void ChangeState(GameState newState)
    {
        Debug.Log("Game State changed from " + gameState + " to " + newState);
        gameState = newState;
    }
}
