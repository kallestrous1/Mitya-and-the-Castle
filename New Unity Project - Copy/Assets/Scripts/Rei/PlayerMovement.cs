using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator ani;

    public float acceleration;
    public float maxSpeed;

    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float midAirXMultiplier;

    public float rememberGroundedFor;
    float lastTimeGrounded;
   

    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float previousXMovement = 1;
    public static int rotationX = 1;
    PlayerHealth playerHealth;

    public Vector2 outsideForce;
    public float forceDecay = 1f;
    public float knockBackForce = 10f;

    bool knocked = false;
    float knockTime = 0.5f;
    float currentKnockTime;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {     
        BetterJump();
        CheckIfGrounded();      
        ani.SetBool("isGrounded", isGrounded);
        outsideForce = Vector2.Lerp(outsideForce, Vector2.zero, forceDecay * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (currentKnockTime <= 0)
        {
            knocked = false;
        }

        if (knocked == false)
        {
            moveAndRotate();
        }
        else 
        {
            currentKnockTime -= Time.deltaTime;
        }
    }
   
    void BetterJump()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (rb.velocity.y < 0)
        {                     
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Z))
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    } 

    public static int getDirection()
    {
        return rotationX;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D otherBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 knockBackDirection = otherBody.transform.position - this.transform.position;
            knockBackDirection.Normalize();
            knockBackDirection.x = knockBackDirection.x * knockBackForce*7;
            knockBackDirection.y = knockBackDirection.y * knockBackForce*10;
            addOutsideForce(-knockBackDirection);
            knock();

        }
    }
    public void addOutsideForce(Vector2 force)
    {
        outsideForce += force;
        rb.velocity += outsideForce;
    }

    public void moveAndRotate()
    {
        Vector2 newVelocity = Vector2.zero;
        float xMovement = Input.GetAxis("Horizontal");
        if (xMovement < 0)
        {
            rotationX = -1;
        }
        else if (xMovement > 0)
        {
            rotationX = 1;
        }

        ani.SetFloat("MoveX", xMovement);
        ani.SetFloat("rotationX", rotationX);

        if (isGrounded)
        {
            newVelocity = new Vector2(xMovement * maxSpeed, rb.velocity.y);

        }
        else
        {
            newVelocity = new Vector2(xMovement * maxSpeed * midAirXMultiplier, rb.velocity.y);

        }
        if (knocked == false)
        {
            rb.velocity = newVelocity;
        }
    }
    void knock()
    {
        currentKnockTime = knockTime;
        knocked = true;
    }
}
