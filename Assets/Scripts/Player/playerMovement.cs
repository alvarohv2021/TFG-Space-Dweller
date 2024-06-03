using System;
using System.Collections;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public interfaceHandler interfaces;
    public int numOfLives = 3;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded { get; set; }
    public bool isWalled { get; set; }

    private Rigidbody2D rb;
    public Animator animator;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (interfaces == null)
        {
            interfaces = FindObjectOfType<interfaceHandler>();
        }
    }

    private void Update()
    {
        Move();
        Jump();
        Pause();
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            interfaces.TagglePause();
        }
    }

    private void Move()
    {
        if (!Input.GetKey(KeyCode.W) && !isWalled)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            animator.SetFloat("Speed", Mathf.Abs(moveInput));

            if (moveInput > 0 && !facingRight)
            {
                Rotate();
            }
            else if (moveInput < 0 && facingRight)
            {
                Rotate();
            }
        }
        else if (isWalled && !isGrounded)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }
        else if(!Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }
        animator.SetBool("IsJumping", !isGrounded);
    }

    private void Rotate()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
