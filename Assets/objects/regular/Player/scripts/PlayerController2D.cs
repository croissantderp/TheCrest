using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    //various components
    private Rigidbody2D Rigidbody2D;

    private Animator animator;

    //movement values
    public float move;

    public float speed;

    public float maxSpeed = 10f;

    public float otherMultiplier = 1;

    //jumping values
    float horizontalJump;

    float verticalJump;

    float crouchJumpMultiplier;

    float horizontalJumpSpeed = 7.5f;

    float verticalJumpSpeed = 20f;

    float directionMultiplier;

    //various states
    private bool facingRight = true;

    private bool grounded;

    private bool climbing;

    bool crouch;

    //colliders and checks
    public Collider2D CrouchDisableCollider;

    public Collider2D CrouchEnableCollider;

    public Transform CeilingCheck;

    public Transform GroundCheck;

    public Transform GroundCheck2;

    public Transform GroundCheck3;

    public LayerMask WhatIsGround;

    public Transform WallCheck;

    public LayerMask WhatIsWall;

    //check if player should be destroyed
    public bool destroy = false;

    //timer
    float timeRemaining;

    //if the player can accelerate
    bool acceleratable, acceleratablel;

    //if player can move
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        //getting components
        Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        //disables cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //makes sure player isn't destroyed between scenes
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        //animation
        animator.SetBool("IsClimbing", climbing);

        //moving
        if (Rigidbody2D.velocity.x != 0 && grounded && !climbing)
        {
            if (Input.GetKey("left") || Input.GetKey("right"))
            {
                animator.SetBool("IsMoving", true);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        //moving vertically
        if (Rigidbody2D.velocity.y != 0 && climbing)
        {
            if (Input.GetKey("up") || Input.GetKey("down"))
            {
                animator.SetBool("VIsMoving", true);
            }
            else
            {
                animator.SetBool("VIsMoving", false);
            }
        }

        //crouching
        animator.SetBool("IsCrouching", crouch);

        //jumping
        if (Input.GetKey("space"))
        {
            animator.SetBool("IsJumping", true);
        }
        else if (grounded || climbing)
        {
            animator.SetBool("IsJumping", false);
        }

        //if the gameobject should be destroyed
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if player can move
        if (canMove)
        {
            //checking if grounded
            if (!crouch && Physics2D.OverlapCircle(GroundCheck.position, 1f, WhatIsGround))
            {
                grounded = true;
            }
            else if (!crouch)
            {
                grounded = false;
            }

            //calculates speed
            calculate();

            //movement
            if (Input.GetKey("right") || Input.GetKey("left"))
            {
                move2();
            }

            //jumping with cooldown clock
            if (timeRemaining <= 0)
            {
                if (Input.GetKey("space"))
                {
                    jump();
                }
            }
            else
            {
                timeRemaining -= Time.deltaTime;
            }

            //crouching
            if (!climbing)
            {
                //crouch and uncrouch
                if (Input.GetKey("s"))
                {
                    crouch = true;
                }
                else if (Input.GetKey("w"))
                {
                    crouch = false;
                    if (Physics2D.OverlapCircle(CeilingCheck.position, 0.5f, WhatIsGround))
                    {
                        crouch = true;
                    }
                }
                crouching();
            }

            //climbing
            if (Physics2D.OverlapCircle(WallCheck.position, 1f, WhatIsWall) && !crouch)
            {
                climbing = true;
                climb();
                //adjusting physics
                Rigidbody2D.gravityScale = 0;
                Rigidbody2D.drag = 7;
            }
            else
            {
                climbing = false;
                Rigidbody2D.gravityScale = 3;
                Rigidbody2D.drag = 0;
            }
        }  
    }

    //movement
    private void move2()
    {
        //changing current speed to max speed if current speed is faster than the max speed
        if (facingRight || Input.GetKey("right"))
        {
            acceleratable = (speed <= maxSpeed);
        }

        if (!facingRight || Input.GetKey("left"))
        {
            acceleratablel = (speed >= -maxSpeed);
        }

        //movement left and right
        if (acceleratable && Input.GetKey("right"))
        {
            move = 1;
            Rigidbody2D.velocity = new Vector2(speed, Rigidbody2D.velocity.y);
            flip();
        }
        if (acceleratablel && Input.GetKey("left"))
        {
            move = -1;
            Rigidbody2D.velocity = new Vector2(speed, Rigidbody2D.velocity.y);
            flip();
        }
    }

    //how to flip the character
    private void flip()
    {
        if (!facingRight && Rigidbody2D.velocity.x > 0)
        {
            Vector3 characterScale = transform.localScale;
            characterScale.x *= -1;
            transform.localScale = characterScale;
            facingRight = true;
        }
        if (facingRight && Rigidbody2D.velocity.x < 0)
        {
            Vector3 characterScale = transform.localScale;
            characterScale.x *= -1;
            transform.localScale = characterScale;
            facingRight = false;
        }
    }

    //seperate loop for jumping
    private void jump()
    {
        //can player jump
        if (climbing || grounded)
        {
            //checks if player is facing right and reverses the numbers in jumping;
            if (climbing)
            {
                if (Input.GetKey("left") || Input.GetKey("right"))
                {
                    directionMultiplier = (facingRight ? 1 : -1);
                }
                else
                {
                    directionMultiplier = (facingRight ? -1 : 1);
                }
            }
            else
            {
                directionMultiplier = (facingRight ? 1 : -1);
            }

            //checks if player is crouching or climbing
            crouchJumpMultiplier = (crouch ? 1.2f : climbing ? 1.5f : 1);

            //multiplies variables together
            horizontalJump = crouchJumpMultiplier * horizontalJumpSpeed * directionMultiplier;
            verticalJump = crouchJumpMultiplier * verticalJumpSpeed;

            Rigidbody2D.velocity = new Vector2(horizontalJump, verticalJump);

            //flips the player
            flip();

            //resets timer
            timeRemaining = 0.5f;
        }
    }

    //function for climbing
    private void climb()
    {
        if (Input.GetKey("down"))
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, -10f);
        }
        if (Input.GetKey("up"))
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 10f);
        }
    }

    //function for crouching
    private void crouching()
    {
        //disable or enable colliders
        if (crouch)
        {
            if (CrouchDisableCollider != null)
                CrouchDisableCollider.enabled = false;
            if (CrouchEnableCollider != null)
                CrouchEnableCollider.enabled = true;

            //also checks if grounded using 2 colliders when crouching
            if (Physics2D.OverlapCircle(GroundCheck2.position, 0.6f, WhatIsGround))
            {
                grounded = true;
            }
            else if (Physics2D.OverlapCircle(GroundCheck3.position, 0.6f, WhatIsGround))
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
        else
        {
            if (CrouchDisableCollider != null)
                CrouchDisableCollider.enabled = true;
            if (CrouchEnableCollider != null)
                CrouchEnableCollider.enabled = false;
        }
    }

    //math for movement
    void calculate()
    {
        //speed player up while crouching
        float crouchSpeed = (crouch ? 1f : 0.75f);

        //assigning local variables
        float attackSpeed;
        float flyingSpeed;

        //increases speed cap when flying
        float flySpeed = (grounded ? 1f : 1.25f);

        //slow player down while flying
        if (grounded)
        {
            flyingSpeed = 1f;
        }
        else
        {
            flyingSpeed = (crouch ? 0.25f : 0.5f);
        }

        //slow player down while attacking
        if (Input.GetKey("x"))
        {
            attackSpeed = 0.5f;
        }
        else
        {
            //slows player down while bracing
            attackSpeed = (Input.GetKey("z") ? 0.75f : 1f);
        }

        //adjust maxspeed based on conditions
        maxSpeed = 13f * otherMultiplier * attackSpeed * crouchSpeed * flySpeed;

        //multiply various variables together to make speed
        speed = Rigidbody2D.velocity.x + move * crouchSpeed * flyingSpeed * attackSpeed * otherMultiplier;
    }
}