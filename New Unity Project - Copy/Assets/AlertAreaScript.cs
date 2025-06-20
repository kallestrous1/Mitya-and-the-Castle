using UnityEngine;

public class AlertAreaScript : MonoBehaviour
{

    public bool playerDetected = false;
    public GameObject Player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            Player = other.gameObject;
            playerDetected = true;
        }
    }
}
