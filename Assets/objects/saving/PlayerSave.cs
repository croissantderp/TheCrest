using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    //various data
    public int health;
    public string level;
    public float xPos;
    public float yPos;
    public int song;
    public int FlatSong;
    public int SharpSong;
    public bool[] items;
    public string[] itemNames;

    public bool respawn = false;

    //starts the loading function
    public void Loading()
    {
        StartCoroutine(LoadPlayer());
    }

    //the actual saving function
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    //loading function
    public IEnumerator LoadPlayer()
    {
        //passing out data
        PlayerData data = SaveSystem.LoadPlayer();

        yield return new WaitForSeconds(1f);

        xPos = data.position[0];
        yPos = data.position[1];
        level = data.level;
        health = data.health;
        song = data.song;
        FlatSong = data.FlatSong;
        SharpSong = data.SharpSong;
        items = data.items;
        itemNames = data.itemNames;

        //passing scene and coordinate data        
        transition transition = FindObjectOfType<transition>();
        transition.respawning = respawn;
        transition.sceneName = level;
        transition.xPosition = xPos;
        transition.yPosition = yPos;
        transition.LoadNext();

        yield return new WaitForSeconds(0.65f);

        //finds health
        Health hlth = FindObjectOfType<Health>();

        //if respawning
        if (respawn)
        {
            //sets variables
            hlth.currentHealth = hlth.maxHealth;
            hlth.alive = true;
            
            //makes player able to move
            PlayerController2D ply = FindObjectOfType<PlayerController2D>();
            ply.canMove = true;

            respawn = false;
        }
        else
        {
            //sets health otherwise
            hlth.currentHealth = health;
        }

        //sets song
        hlth.song = song;
        hlth.FlatSong = FlatSong;
        hlth.SharpSong = SharpSong;

        hlth.items = items;
        hlth.itemNames = itemNames;
        hlth.ReverseUpdateArmor();
        hlth.SetArmor();

        inventory iv = FindObjectOfType<inventory>();

        iv.UpdateItems();
        iv.UpdateSlots();

        //makes cursor not visible
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
