using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    //if the game is paused
    public bool paused = false;

    //buttons for quit and restart
    public Button Quit;
    public Button Restart;

    //referencing 2 animators
    public Animator Atransition;
    public Animator loseTransition;

    //the time Loader waits
    public float transitionTime = 0.1f;

    //the scene name
    public string scene;

    //the pause menu
    public GameObject menu;

    //if the player loses
    public bool lost;

    // Start is called before the first frame update
    void Start()
    {
        //moving the ui out of the screen range
        Vector3 Scale = menu.transform.localPosition;
        Scale.y = 1000;
        menu.transform.localPosition = Scale;
    }

    // Update is called once per frame
    private void Update()
    {
        //if esc is pressed
        if (!paused && Input.GetKeyDown("escape"))
        {
            //paused time
            Time.timeScale = 0f;
            paused = true;

            //moving the ui into the screen range
            Vector3 Scale = menu.transform.localPosition;
            Scale.y = 0;
            menu.transform.localPosition = Scale;
        }
        //if q is pressed again
        else if (Input.GetKeyDown("escape"))
        {
            //starts time
            Time.timeScale = 1f;
            paused = false;

            //moving the ui out of screen range
            Vector3 Scale = menu.transform.localPosition;
            Scale.y = 1000;
            menu.transform.localPosition = Scale;
        }
    }

    //if quitting
    void Quitting()
    {
        scene = "start menu";
        LoadNext();
    }

    //if restarting
    void Restarting()
    {
        battleLose lose = FindObjectOfType<battleLose>();
        lose.destroyThis();

        //reloads current scene
        Scene scene2 = SceneManager.GetActiveScene();
        scene = scene2.name;
        LoadNext();
    }

    //if player has lost
    public void lose()
    {
        lost = true;
        LoadNext();
    }

    //starts the load function and unpauses time
    public void LoadNext()
    {
        //starts the Loading function
        StartCoroutine(Loading());
        Time.timeScale = 1f;
    }

    //the loading function
    IEnumerator Loading()
    {
        //starts animation
        Atransition.SetTrigger("start");

        //waits, then continues rest of code
        yield return new WaitForSeconds(transitionTime);
        
        //if player has lost
        if (lost)
        {
            battleLose bl = FindObjectOfType<battleLose>();
            bl.loseLoad();
            SceneManager.LoadScene("room 1 enter");
        }
        else
        {        //loads next scene
            SceneManager.LoadScene(scene);
        }
    }
}
