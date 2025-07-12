using UnityEngine;

public class HazardCollider : MonoBehaviour
{
    public int damage;
    public AudioClip hitSound;
    public bool oneOff = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<KnockBack>().isBeingKnockedBack)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                Vector2 knockBackDirection = (collision.transform.position - transform.position).normalized;

                playerHealth.changeHealth(-damage, knockBackDirection);
                

                if (hitSound)
                {
                    AudioManager.Instance.Play(hitSound);
                }
            }
            if (oneOff)
            {
                Destroy(this.transform.gameObject);
            }
        }
        else if(collision.gameObject.tag == "Ground" && oneOff)
        {
            Destroy(this.transform.gameObject);
        }

    }
}
