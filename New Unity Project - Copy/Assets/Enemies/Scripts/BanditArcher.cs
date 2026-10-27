using System.Collections;
using UnityEngine;

public class BanditArcher : Enemy
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed;

    public AudioSource audioSource;
    public AudioClip drawBowSound;
    public AudioClip shootBowSound;

    [Header("Retreat")]
    public float retreatForce = 20f;
    public float retreatDistance = 8f;

    PlayerInRangeDetector rangeDetector;

    Vector2 lockedShootDirection;
    bool isRunningAway;

    public override void Start()
    {
        base.Start();
        rangeDetector = GetComponentInChildren<PlayerInRangeDetector>();
    }

    public override void processHit()
    {
        // Run away when hit, unless we're already fleeing.
        if (!isRunningAway)
            StartCoroutine(RunAway());
        else
            ani.SetTrigger("Stun");
    }

    IEnumerator RunAway()
    {
        isRunningAway = true;
        if (ani)
            ani.SetBool("Walk", true);

        while (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
            if (distanceToPlayer >= retreatDistance)
                break;

            // Push away from the player; gravity handles vertical movement.
            float direction = transform.position.x < target.transform.position.x ? -1f : 1f;
            rb.AddForce(new Vector2(direction * retreatForce, 0f), ForceMode2D.Impulse);

            yield return new WaitForFixedUpdate();
        }

        if (ani)
            ani.SetBool("Walk", false);
        isRunningAway = false;
    }

    public override void OnPlayerLeftRange()
    {
        if (audioSource)
            audioSource.Stop();
        if (ani)
        {
            ani.ResetTrigger("Attack");
            ani.Play("banditarcheridle");
        }
    }

    bool PlayerIsInRange()
    {
        return rangeDetector != null && rangeDetector.playerInRange;
    }

    public void playDrawBow()
    {
        if (!PlayerIsInRange() || isRunningAway)
            return;

        // Face the player and lock in the shot direction before the draw.
        // If the player crosses sides during the animation, we still shoot forward.
        UpdateTurnDirection();
        if (target != null)
            lockedShootDirection = ((Vector2)(target.transform.position - firePoint.position)).normalized;

        if (drawBowSound)
        {
            audioSource.clip = drawBowSound;
            audioSource.Play();
        }
    }

    public void playShootBow()
    {
        if (!PlayerIsInRange() || isRunningAway)
            return;
        if (shootBowSound)
        {
            audioSource.clip = shootBowSound;
            audioSource.Play();
        }
    }

    public void ShootArrow()
    {
        if (!PlayerIsInRange() || isRunningAway)
            return;

        // Use the direction locked in at draw time so the shot commits "forward".
        // Fall back to the live direction if the draw event never ran.
        Vector2 shootDirection = lockedShootDirection;
        if (shootDirection == Vector2.zero)
            shootDirection = ((Vector2)(target.transform.position - firePoint.position)).normalized;

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.Euler(0,0 , angle));

        // Arrows pass through every enemy, including the archer that fired them.
        Collider2D arrowCol = arrow.GetComponent<Collider2D>();
        if (arrowCol != null)
        {
            foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            {
                foreach (Collider2D enemyCol in enemy.GetComponentsInChildren<Collider2D>())
                    Physics2D.IgnoreCollision(arrowCol, enemyCol);
            }
        }

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(shootDirection * arrowSpeed, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("no rigidbody found on arrow");
        }
        Destroy(arrow, 4f);
    }
}
