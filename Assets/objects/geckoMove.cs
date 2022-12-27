using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class geckoMove : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform groundCheck;
    public Transform wallCheck;

    public Transform sprite;

    public Collider2D crouch;
    public Collider2D regular;

    public Animator anim;

    float moveDirection;
    float VerticalDirection;

    public float maxSpeed;

    public bool isGrounded;
    bool isClimbing;
    bool isCrouching;

    bool crouchButtonDown;
    bool crouchButtonWasDown;
    bool jumpButtonDown;

    public LayerMask WhatIsGround;

    bool canJump;

    public bool canMove;

    public Transform crouchMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            OnMove(moveDirection);

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.25f, WhatIsGround);
            isClimbing = Physics2D.OverlapCircle(wallCheck.position, 0.5f, WhatIsGround);

            if (isGrounded)
            {
                canJump = true;
            }

            if (isClimbing)
            {
                climb(VerticalDirection);
                rb.gravityScale = 0;
                rb.drag = 5;
            }
            else
            {
                rb.gravityScale = 1;
                rb.drag = 0;

                if (crouchButtonDown)
                {
                    if (!isGrounded)
                    {
                        rb.AddForce(new Vector2(0, -10f));
                        crouchButtonWasDown = true;
                    }
                    else if (isGrounded && !crouchButtonWasDown)
                    {
                        if (!isCrouching)
                        {
                            isCrouching = true;

                            crouchButtonDown = false;
                        }
                        else if (isCrouching)
                        {
                            isCrouching = false;

                            crouchButtonDown = false;
                        }
                    }
                }

                Crouch();

                if (jumpButtonDown)
                {
                    Jump();
                }
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<float>();
    }

    public void OnMove(float direction)
    {
        if (direction == 1 && rb.velocity.x <= maxSpeed)
        {
            rb.AddForce(new Vector2(direction * 40, 0));
            sprite.localScale = new Vector3(0.75f, 0.75f, 1);
        }

        if (direction == -1 && rb.velocity.x >= -maxSpeed)
        {
            rb.AddForce(new Vector2(direction * 40, 0));
            sprite.localScale = new Vector3(-0.75f, 0.75f, 1);
        }
    }

    public void vertical(InputAction.CallbackContext context)
    {
        VerticalDirection = context.ReadValue<float>();
    }

    public void climb(float direction)
    {
        if (Mathf.Abs(rb.velocity.y) <= maxSpeed)
        {
            rb.AddForce(new Vector2(0, direction * 25));
        }
    }

    public void jumpPress(InputAction.CallbackContext context)
    {
        jumpButtonDown = (context.ReadValue<float>() == 1 ? true : false);
    }

    public void Jump()
    {
        if (canJump)
        {
            rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
            canJump = false;
        }
        else
        {
            rb.AddForce(new Vector2(0, 5f));
        }
    }

    public void crouchPress(InputAction.CallbackContext context)
    {
        crouchButtonDown = (context.ReadValue<float>() == 1 ? true : false);
        crouchButtonWasDown = false;
    }

    public void Crouch()
    {
        if (isCrouching && isGrounded)
        {
            crouch.enabled = true;
            regular.enabled = false;
            anim.SetBool("crouch", true);

            crouchMove.position = Vector3.Lerp(crouchMove.position, transform.position + new Vector3(0, -1f, 0), 0.25f);
        }
        else
        {
            crouch.enabled = false;
            regular.enabled = true;
            anim.SetBool("crouch", false);

            crouchMove.position = Vector3.Lerp(crouchMove.position, transform.position + new Vector3(0, 0, 0), 0.25f);
        }
    }
}
