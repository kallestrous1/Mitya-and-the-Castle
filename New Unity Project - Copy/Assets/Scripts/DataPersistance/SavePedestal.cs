using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePedestal : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Vector2 playerSpawnPoint;
    public string playerSpawnScene;


    [Header("Effects")]
    [SerializeField] ParticleSystem saveEffect;
    [SerializeField] AudioClip saveSound;

    bool saveRequest;
    private bool playerInRange = false;

    private void Awake()
    {
        // Cache reference 
        if (saveEffect == null)
            saveEffect = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetButtonDown("Interact"))
        {
            PerformSave();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            playerInRange = false;
        }
    }

    private void PerformSave()
    {
        // Visuals
        if (saveEffect != null)
        {
            saveEffect.Simulate(0f, true, true);
            saveEffect.Play();
        }

        // Audio
        if (saveSound != null)
            AudioManager.Instance.Play(saveSound);

        // Heal Player
        var playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.changeHealth(playerHealth.maxHealth);

        var playerMagic = FindAnyObjectByType<PlayerMagicJuice>();
        if (playerMagic != null)
            playerMagic.changeMagic(PlayerMagicJuice.maxMagic);

        // Set respawn point using your manager
        NewManager.manager.defaultPlayerLocation = playerSpawnPoint;
        NewManager.manager.defaultPlayerScene = playerSpawnScene;

        // Save to persistence system
        DataPersistenceManager.instance.SaveGame();
    }
}
