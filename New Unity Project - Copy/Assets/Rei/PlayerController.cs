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
    PlayerSound playerSound;

    public AudioClip dashSound;

    public GameObject doubleJumpEffect;

    private KnockBack knockback;


    void Start()
    {
       
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        ani = this.gameObject.GetComponent<Animator>();
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
        playerAttacks = this.gameObject.GetComponent<PlayerAttacks>();
        knockback = this.gameObject.GetComponent<KnockBack>();
        this.playerSound = this.gameObject.GetComponent<PlayerSound>();
        this.transform.position = NewManager.playerSaveLocation;
    }
    private void Update()
    {
        if (NewManager.manager.currentGameState == GameState.Paused)
        {
            return;
        }

        if (knockback.isBeingKnockedBack)
        {
            return;
        }

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
        if ((Input.GetButton("Vertical")) && (isGrounded == false) && (rb.linearVelocity.y > 0))
        {
            boost = true;
        }
        else
        {
            boost = false;
        }

        if (Input.GetButton("ChangeWeapon"))
        {
            //playerAttacks.setWeapon(1);
        }

    }

    void FixedUpdate()
    {
        if(DialogueManager.getInstance().dialogueIsPlaying)
        {
            return;
        }

        if (knockback.isBeingKnockedBack)
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
            if (rb.linearVelocity.y > 0) { 
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
                Vector2 currentPosition = this.transform.position;
                this.transform.position = new Vector2(currentPosition.x + 1, currentPosition.y);
                this.transform.Rotate(0f, 180f, 0f);
            }
            rotationX = 1;
        }
        else if (xInput< 0)
        {
            if (flipped)
            {

                flipped = false;     
                
                Vector2 currentPosition = this.transform.position;
                this.transform.position = new Vector2(currentPosition.x - 1, currentPosition.y);
                this.transform.Rotate(0f, -180f, 0f);
            }
            rotationX = -1;
        }
        if (!isDashing)
        {
            if (isGrounded)
            {
                rb.AddForce(new Vector2(xInput * speed, 0), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(xInput * speed * 0.5f, 0), ForceMode2D.Impulse);
            }
        }
    }
    #endregion

    #region Dash
    IEnumerator Dash()
    {
        if (dashCount > 0)
        {
            dashCount--;
            playerSound.PlayDashSound();
            isDashing = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
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
    public void Jump()
    {
        if ((isGrounded || coyoteTimer>0)&&isDashing==false)
        {
            playerSound.PlayJumpSound();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            totalJumps++;
        }
        else if (jumpCount > 0 && isDashing == false)
        {
            jumpCount--;
            if (doubleJumpEffect)
            {
                Instantiate(doubleJumpEffect, isGroundedChecker.position, Quaternion.identity);
            }
            playerSound.PlayExtraJumpSound();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
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

    public void SetToSavedLocation()
    {
        this.transform.position = NewManager.playerSaveLocation;
    }

    public void LoadData(GameData data)
    {
        this.totalJumps = data.totalJumps;
    }

    public void SaveData(GameData data)
    {
        data.totalJumps = this.totalJumps;
    }

}
