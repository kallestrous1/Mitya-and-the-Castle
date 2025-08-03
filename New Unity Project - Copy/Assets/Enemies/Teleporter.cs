using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleporter : Interactable
{
    bool teleRequest;
    public AudioClip teleSound;
    public Vector2 teleLocation;

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            teleRequest = true;

        }
        if (Input.GetButtonUp("Interact"))
        {
            teleRequest = false;

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (teleRequest)
            {
                teleRequest = false;
                if (teleSound)
                {
                    AudioManager.Instance.Play(teleSound);
                }
                StartCoroutine(teleport(collision.GetComponent<Transform>()));
            }
        }
    }

    private IEnumerator teleport(Transform player)
    {
        yield return new WaitForSeconds(0.5f);
       player.position = teleLocation;
    }
}
