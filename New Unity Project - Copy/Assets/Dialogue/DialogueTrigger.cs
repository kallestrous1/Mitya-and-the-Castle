using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (Input.GetButtonDown("Interact") && !DialogueManager.getInstance().dialogueIsPlaying)
            {
                Debug.Log("starting dialogue");
                StartCoroutine(talk());
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private IEnumerator talk()
    {
        yield return new WaitForSeconds(0.2f);
        DialogueManager.getInstance().EnterDialogueMode(inkJSON);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

}
