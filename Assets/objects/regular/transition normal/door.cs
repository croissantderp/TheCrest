using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    //transition script
    public transition ts;

    //scene name
    public string scene;

    //player new location
    public float x;
    public float y;

    //if door to going right
    public bool right;

    //makes sure load function is only called once
    public bool done = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!done)
        {
            //if player collides with door, load next scene
            if (collision.gameObject.CompareTag("Player"))
            {
                ts = FindObjectOfType<transition>();
                //passes information
                ts.right = right;
                ts.xPosition = x;
                ts.yPosition = y;
                ts.sceneName = scene;
                ts.LoadNext();
                done = true;
            }
        }   
    }
}
