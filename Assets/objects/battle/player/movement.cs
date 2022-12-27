using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    //components
    public Rigidbody2D rb;

    //speed
    public float speed;

    //how much to accelerate by
    public float acceleration;

    bool acceleratable;
    bool acceleratablel;

    //if grounded
    public bool grounded;

    //if crouching
    public bool crouching;

    //the collider to disable on crouch
    public Collider2D capsule;

    //if can move
    public bool canMove = false;

    //checks for grounded
    public Transform GroundCheck;

    public LayerMask WhatIsGround;

    public bool movingL;
    public bool movingR;

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
            //finding current speed
            speed = rb.velocity.x;

            //checking if grounded
            grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.25f, WhatIsGround);

            if (!Keyboard.current.rightArrowKey.isPressed || !Keyboard.current.leftArrowKey.isPressed)
            {
                //if speed isn't over the cap
                if (speed <= 7.5f && Keyboard.current.rightArrowKey.isPressed)
                {
                    rb.AddForce(transform.right * 1, ForceMode2D.Impulse);
                    movingR = true;
                }
                else
                {
                    movingR = false;
                }

                //if speed isn't over the cap
                if (speed >= -7.5f && Keyboard.current.leftArrowKey.isPressed)
                {
                    rb.AddForce(transform.right * -1, ForceMode2D.Impulse);
                    movingL = true;
                }
                else
                {
                    movingL = false;
                }
            }

            if (!Keyboard.current.upArrowKey.isPressed || !Keyboard.current.downArrowKey.isPressed)
            {
                //jumping
                if (Keyboard.current.upArrowKey.isPressed)
                {
                    if (grounded)
                    {
                        rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(transform.up * 0.1f, ForceMode2D.Impulse);
                    }
                }

                if (Keyboard.current.downArrowKey.isPressed)
                {
                    //downforce
                    if (!grounded)
                    {
                        rb.AddForce(transform.up * -1, ForceMode2D.Impulse);
                    }

                    //crouching
                    crouching = true;
                    capsule.enabled = false;

                    //adjusts ground check to crouch position
                    GroundCheck.localPosition = new Vector3(0, -0.5f, 0);
                }
                else
                {
                    //crouching
                    crouching = false;
                    capsule.enabled = true;

                    //adjusts ground check to crouch position
                    GroundCheck.localPosition = new Vector3(0, -1.5f, 0);
                }
            }
        }
    }
}
