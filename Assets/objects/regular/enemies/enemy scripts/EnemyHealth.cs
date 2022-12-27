using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //health values
    public float maxHealth;
    public float currentHealth;

    //player collision checkers
    public Transform PlayerCheck;
    public LayerMask WhatIsPlayer;

    //timer component
    public float timeRemaining = 1;

    //player object
    public GameObject player;

    public bool destroyed = false;

    public int damageDealt;

    //speed of the enemy
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        //finds the player after entering a new scene
        player = GameObject.Find("player");
        //the max health value
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //moves enemy at player
        Vector3 localPosition = player.transform.position - transform.position;
        localPosition = localPosition.normalized; // The normalized direction in LOCAL space
        transform.Translate(localPosition.x * Time.deltaTime * speed, localPosition.y * Time.deltaTime * speed, localPosition.z * Time.deltaTime * speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //timer for health
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            //checks if player is close and subtracts damage from health
            if (Physics2D.OverlapCircle(PlayerCheck.position, 1f, WhatIsPlayer))
            {
                if (Input.GetKey("x") && !Input.GetKey("z"))
                {
                    currentHealth -= 1;
                    timeRemaining = 1;
                }
            }
        }

        //checks if health is lower than 0 and destroys enemy
        if (currentHealth <= 0)
        {
            Destroy(gameObject, 0.5f);
            destroyed = true;
        }

        //checks if player is being attacked
        if (Physics2D.OverlapCircle(PlayerCheck.position, 1f, WhatIsPlayer) && player != null && !destroyed)
        {
            Health health = FindObjectOfType<Health>();
            health.beingAttacked = true;
            health.damageTaken = damageDealt;
        }
        else if (player != null)
        {
            Health health = FindObjectOfType<Health>();
            health.beingAttacked = false;
        }

    }

    //subtract damage from health
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
