using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public string level;
    public float[] position;
    public int song;
    public int FlatSong;
    public int SharpSong;
    public string[] itemNames;
    public bool[] items;

    public PlayerData (PlayerSave player)
    {
        health = player.health;
        level = player.level;
        song = player.song;
        FlatSong = player.FlatSong;
        SharpSong = player.SharpSong;
        itemNames = player.itemNames;
        items = player.items;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;      
    }
}
