using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneDoorScript : MonoBehaviour
{
    public Vector2 newPlayerPosition;
    public bool upBoost = false;
    public string nextScene;
    public string previousScene;
    bool loaded;
    bool unloaded;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Player") { return; }
        Rigidbody2D playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerRB.isKinematic = true;
        playerRB.linearVelocity = new Vector2(0, 0);
        other.transform.position = newPlayerPosition;

        if (!loaded)
        {
            loaded = true;
            NewManager.manager.MoveToScene(nextScene, previousScene, upBoost);          
        }
    }
    }
