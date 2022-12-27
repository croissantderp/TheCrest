using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //max health
    public int maxHealth;

    //current health
    public int currentHealth;

    //percentage of currenthealth to maxhealth
    public float healthMultiplier;

    //amount to heal
    public int heal = 2;

    //amount of damage done
    public int damageDealt;

    //amount of damage taken
    public int damageTaken;

    //enemy check and what is enemy
    public Transform EnemyCheck;
    public LayerMask WhatIsEnemy;

    //timer components
    float timeRemaining = 1;
    float timeRemaining2 = 1;

    //if player is alive
    public bool alive = true;

    //if player is being attacked
    public bool beingAttacked;

    //amount of song
    public int song;

    //amount of total song
    public int FlatSong;
    public int SharpSong;

    //various scripts
    public HealthBar healthBar;
    public deathScreen death;
    public PlayerController2D play;

    //points
    public points pts;

    //if the player has items
    public bool[] items = { false, false, false, false, false, false, false };

    public string[] itemNames = { "" , "", "", "", "", "", "" };

    //amount of items currently in slots
    public int[] itemAmounts = { 0, 0, 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        //finds scripts
        pts = FindObjectOfType<points>();
        healthBar = FindObjectOfType<HealthBar>();
        death = FindObjectOfType<deathScreen>();

        //sets armor
        SetArmor();

        //sets the health values to max
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        //updates armor
        ReverseUpdateArmor();
    }

    // Update is called once per frame
    void Update()
    {
        //sets health to current values
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            healthBar = FindObjectOfType<HealthBar>();
        }

        //finds points if it is null
        if (pts != null)
        {
            //gives point script point values
            pts.Change(song, FlatSong, SharpSong);
        }
        else
        {
            pts = FindObjectOfType<points>();
        }

        //finds death is it is null
        if (death == null)
        {
            death = FindObjectOfType<deathScreen>();
        }

        //first timer for damage
        if (timeRemaining <= 0)
        {
            if (beingAttacked && currentHealth > 0)
            {
                if (Input.GetKey("x"))
                {
                    TakeDamage(damageTaken);
                }
                else if (Input.GetKey("z"))
                {
                    TakeDamage(damageTaken * 3 / 4);
                }
                else
                {
                    TakeDamage(damageTaken);
                }
                timeRemaining = 1;
            }
        }
        else
        {
            timeRemaining -= Time.deltaTime;
        }

        //second timer for health
        if (timeRemaining2 <= 0)
        {
            //healing
            if (currentHealth < maxHealth && alive)
            {
                currentHealth += heal;
                timeRemaining2 = 1;
            }

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        else
        {
            timeRemaining2 -= Time.deltaTime;
        }

        //checks health values
        if (currentHealth <= 0)
        {
            alive = false;
        }

        //show death screen when dead, linked to dieScreen code, then destroys player
        if (!alive)
        {
            //makes deathscreen appear
            beingAttacked = false;

            //makes sure player can't move
            PlayerController2D ply = FindObjectOfType<PlayerController2D>();
            ply.canMove = false;
        }
        //makes deathscreen know if player is dead or alive
        death.isAlive = alive;
    }

    //function for saving data
    public void Saving()
    {
        //passes data to save script
        PlayerSave save = FindObjectOfType<PlayerSave>();
        save.health = currentHealth;
        save.song = song;
        save.SharpSong = SharpSong;
        save.FlatSong = FlatSong;
        save.itemNames = itemNames;
        save.items = items;
    }

    //subtract damage from health
    void TakeDamage (int damage)
    {
        currentHealth -= damage; 
    }

    //adds one song
    void SharpSongAdd()
    {
        song = song + 1;
        SharpSong = SharpSong + 1;
    }

    //subtracts one song
    void FlatSongAdd()
    {
        song = song - 1;
        FlatSong = FlatSong + 1;
    }

    //sets health
    public void SetArmor()
    {
        //clears itemAmounts array
        itemAmounts = new int[] { 0, 0, 0, 0, 0, 0, 0};

        //adds the names into the array
        for (int i = 0; i < 6; i++)
        {
            //converts item names into armor amounts
            switch (itemNames[i])
            {
                case "grass":
                    itemAmounts[0] += 1;
                    break;
                case "wood":
                    itemAmounts[1] += 1;
                    break;
                case "spines":
                    itemAmounts[2] += 1;
                    break;
                case "titanium":
                    itemAmounts[3] += 1;
                    break;
                case "mud":
                    itemAmounts[4] += 1;
                    break;
                case "bandage":
                    itemAmounts[5] += 1;
                    break;
                case "dracoSkin":
                    itemAmounts[6] += 1;
                    break;
            }
        }

        //gets percentage of currenthealth
        healthMultiplier = currentHealth / maxHealth;

        //adds armor health to health
        maxHealth = 100 + (itemAmounts[0] * 2) + (itemAmounts[1] * 5) + (itemAmounts[2] * 10) + (itemAmounts[3] * 10) + (itemAmounts[4] * 20) + (itemAmounts[5] * 2) * (itemAmounts[6] * 12);
        heal = 1 + itemAmounts[6];
        damageDealt = 1 + itemAmounts[3] * 2;

        //multiply speed boosts and losses
        play.otherMultiplier = 1 - itemAmounts[3] * 0.05f - itemAmounts[4] * 0.15f + itemAmounts[0] * 0.05f;

        //sets max health on health bar
        healthBar = FindObjectOfType<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        //resets currenthealth based on previous percentages, also rounds it to nearest integer
        currentHealth = (int)Mathf.Round(maxHealth * healthMultiplier);
    }

    //updates inventory ui
    public void ReverseUpdateArmor()
    {
        inventory iv = FindObjectOfType<inventory>();
        iv.itemNames = itemNames;
        iv.items = items;
    }
}
