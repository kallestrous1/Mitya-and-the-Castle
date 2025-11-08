using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerRespawnManager : MonoBehaviour
{
    public static PlayerRespawnManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {

        string spawnScene = NewManager.manager.defaultPlayerScene;
        NewManager.manager.MoveToScene(spawnScene, SceneManager.GetActiveScene().name, false);


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        //   NewManager.manager.MoveToScene("Base Scene", "Base Scene", false);
        if (player)
        {
            var pc = player.GetComponent<PlayerController>();
            pc?.SetToSavedLocation();
            var playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.changeHealth(playerHealth.maxHealth);
        }
        DataPersistenceManager.instance.SaveGame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(spawnScene));
        yield return null;

    }
}