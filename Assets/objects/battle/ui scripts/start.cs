using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //starts the function
        StartCoroutine(stop());
    }

    //starts game
    public void press()
    {
        //starts time
        Time.timeScale = 1f;

        geckoMove move = FindObjectOfType<geckoMove>();
        move.canMove = true;

        //tells enemySpawner it has started
        enemySpawner enemy = FindObjectOfType<enemySpawner>();
        enemy.started = true;
        enemy.started2 = true;

        //moving the ui out of the screen range
        Vector3 Scale = transform.localPosition;
        Scale.y = 1000;
        transform.localPosition = Scale;
    }

    //stops time after entry animation plays
    IEnumerator stop()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0f;
    }
}
