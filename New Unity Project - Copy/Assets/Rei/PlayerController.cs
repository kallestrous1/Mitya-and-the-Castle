using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistence
{

    public int totalJumps = 0;

    Rigidbody2D rb;
    Animator ani;
    public float speed;
    public float jumpHeight;
    public float lowJumpMultiplier = 2f;
    float xInput;

    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public int JUMPCOUNT;
    public int jumpCount;
    bool jumpRequest = false;
    bool boost;

    public float coyoteTime = 0.2f;
    public float coyoteTimer;
    bool coyoteJump;

    public float jumpBufferTime;
    float jumpBufferCounter;

    float rotationX;
    public float dashTime;
    bool isDashing;
    bool dashRequest;
    public float dashPower;
    public float DASHCOUNT;
    float dashCount;

    public float recoilMagnitude = 500;

    bool flipped = false;

    PlayerHealth playerHealth;
    PlayerAttacks playerAttacks;


    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        ani = this.gameObject.GetComponent<Animator>();
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
        playerAttacks = this.gameObject.GetComponent<PlayerAttacks>();

    }
    private void Update()
    {
        CheckIfGrounded();
        xInput = Input.GetAxisRaw("Horizontal");
        ani.SetBool("IsDashing", isDashing);

        if (Input.GetButtonDown("Vertical"))
        {
            jumpRequest = true;
        }
        if (Input.GetButtonDown("Dash")&& !isDashing)
        {
            dashRequest = true;
        }     
        if ((Input.GetButton("Vertical")) && (isGrounded == false) && (rb.velocity.y > 0))
        {
            boost = true;
        }
        else
        {
            boost = false;
        }

        if (Input.GetButton("ChangeWeapon"))
        {
            playerAttacks.setWeapon(1);
        }

    }

    void FixedUpdate()
    {
        if(DialogueManager.getInstance().dialogueIsPlaying)
        {
            return;
        }

        XMovement();
        if (isGrounded == true)
        {
            ani.SetFloat("MoveX", Mathf.Abs(xInput));
            ani.SetFloat("MoveY", 0);         
            dashCount = DASHCOUNT;
            jumpCount = JUMPCOUNT;
            coyoteTimer = coyoteTime;
        }
        else
        {
            if (rb.velocity.y > 0) { 
                ani.SetFloat("MoveY", 1);
                ani.SetFloat("MoveX", 0);
            }
            else
            {
                ani.SetFloat("MoveY", 0);
                ani.SetFloat("MoveX", 0);
            }
            coyoteTimer -= Time.deltaTime;
        }

        if (jumpRequest == true)
        {
            jumpRequest = false;
            Jump();
            coyoteTimer = 0;
            jumpRequest = false;
        }
        if (boost==true)
        {
            rb.AddForce(new Vector2(0, lowJumpMultiplier), ForceMode2D.Impulse);
        }
        if (dashRequest == true)
        {
            dashRequest = false;
            StartCoroutine(Dash());
        }
    }

    #region XMovementandRotation
    void XMovement()
    {

        if (xInput > 0)
        {
            if (!flipped)
            {               
                flipped = true;               
              
                this.transform.Rotate(0f, 180f, 0f);

                Vector2 currentPosition = this.transform.position;
                this.transform.position = new Vector2(currentPosition.x + 1, currentPosition.y);
            }
            rotationX = 1;
        }
        else if (xInput< 0)
        {
            if (flipped)
            {
     
                flipped = false;     
                
                this.transform.Rotate(0f, -180f, 0f);

                Vector2 currentPosition = this.transform.position;
                this.transform.position = new Vector2(currentPosition.x - 1, currentPosition.y);
            }
            rotationX = -1;
        }
        if (!isDashing)
        {
            rb.AddForce(new Vector2(xInput * speed, 0), ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Dash
    IEnumerator Dash()
    {
        if (dashCount > 0)
        {
            dashCount--;
            isDashing = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(rotationX * dashPower, 0), ForceMode2D.Impulse);
            float gravity = rb.gravityScale;
            rb.gravityScale = 0;
            yield return new WaitForSeconds(dashTime);
            isDashing = false;
            rb.gravityScale = gravity;
        }
    }
    void Dashx()
    { 
        rb.AddForce(new Vector2(rotationX * dashPower, 0), ForceMode2D.Impulse);

    }
    #endregion

    #region Jump
    void Jump()
    {
        if ((isGrounded || coyoteTimer>0)&&isDashing==false)
        {         
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            totalJumps++;
        }
        else if (jumpCount > 0 && isDashing == false)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        }
    }
    #endregion

    #region CheckIfGrounded
    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    #endregion

    #region EnemyCollision

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if (!playerHealth.recovering)
            {
                playerHealth.changeHealth(-1);
                var force = transform.position - collision.transform.position;
                force.Normalize();
                rb.AddForce(force * recoilMagnitude, ForceMode2D.Impulse);
            }
        }
    }
    #endregion


    public void LoadData(GameData data)
    {
        this.totalJumps = data.totalJumps;
    }

    public void SaveData(GameData data)
    {
        data.totalJumps = this.totalJumps;
    }

}
