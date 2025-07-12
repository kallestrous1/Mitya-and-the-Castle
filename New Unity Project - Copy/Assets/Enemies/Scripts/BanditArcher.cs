using UnityEngine;

public class BanditArcher : Enemy
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed;
    

    public override void processHit()
    {
        ani.SetTrigger("Stun");
    }

    public void ShootArrow()
    {
        Debug.Log("shooting arrow!!");

        Vector2 shootDirection = (target.transform.position - firePoint.transform.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.Euler(0,0 , angle));

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
