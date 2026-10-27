using UnityEngine;

public class PlayerInRangeDetector : MonoBehaviour
{
    public bool playerInRange = false;
    public GameObject Player;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exited range of" + this.gameObject.name);
            Player = other.gameObject;
            playerInRange = false;
            Enemy enemy = GetComponentInParent<Enemy>();
            if (enemy)
                enemy.OnPlayerLeftRange();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player entered range of" + this.gameObject.name);
            Player = other.gameObject;
            playerInRange = true;
        }
    }
}
