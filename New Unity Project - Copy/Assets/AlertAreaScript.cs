using UnityEngine;

public class AlertAreaScript : MonoBehaviour
{

    public bool playerDetected = false;
    public bool releaseWhenPlayerExits;
    public GameObject Player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            Player = other.gameObject;
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!releaseWhenPlayerExits || other.tag != "Player")
            return;

        playerDetected = false;
        Enemy enemy = GetComponentInParent<Enemy>();
        if (enemy)
            enemy.OnPlayerLeftRange();
    }
}
