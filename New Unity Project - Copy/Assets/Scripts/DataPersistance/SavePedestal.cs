using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePedestal : MonoBehaviour
{
    public Vector2 playerSpawnPoint;
    public string playerSpawnScene;
    bool saveRequest;

    public AudioClip saveSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            saveRequest = true;
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (saveRequest)
            {
                saveRequest = false;
                this.GetComponentInChildren<ParticleSystem>().Simulate(0.0f, true, true);
                this.GetComponentInChildren<ParticleSystem>().Play();
                AudioManager.Instance.Play(saveSound);
                NewManager.playerSaveLocation = playerSpawnPoint;
                NewManager.playerSpawnScene = this.playerSpawnScene;
                
            }
        }
    }
}
