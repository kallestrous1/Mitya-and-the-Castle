using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleporter : Interactable
{
    [SerializeField] private AudioClip teleSound;
    [SerializeField] private Vector2 teleLocation;
    [SerializeField] private float delay = 0.5f;

    private bool teleRequest = false;
    private PlayerController playerInRange;

    void Update()
    {
        if (playerInRange == null) return;

        if (Input.GetButtonDown("Interact"))
            teleRequest = true;

        if (Input.GetButtonUp("Interact"))
            teleRequest = false;

        if (teleRequest)
        {
            teleRequest = false;
            StartCoroutine(Teleport(playerInRange.transform));
        }

    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
            playerInRange = player;
    }

    new private void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && player == playerInRange)
            playerInRange = null;
    }

    private IEnumerator Teleport(Transform player)
    {
        if (teleSound != null)
            AudioManager.Instance.Play(teleSound);

        yield return new WaitForSeconds(delay);

        if (player != null)
            player.position = teleLocation;
    }
}
