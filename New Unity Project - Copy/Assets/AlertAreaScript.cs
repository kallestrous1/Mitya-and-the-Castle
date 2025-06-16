using UnityEngine;

public class AlertAreaScript : MonoBehaviour
{

    public bool playerDetected = false;
    public GameObject Player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            Debug.Log("playerdetected");
            Player = other.gameObject;
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("plauer out of sight");
            playerDetected = false;
        }
    }
}
