using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transition : MonoBehaviour
{
    //names and positions
    public string sceneName;
    public string currentScene;

    //the coordinates the player is transformed to after the transition
    public float xPosition;
    public float yPosition;

    //UI gameobject
    public GameObject UI;

    //referencing an animator
    public Animator Atransition;

    //the time LoadLevel waits
    public float transitionTime = 0.25f;

    //the player object
    public GameObject player;

    public Health health;

    public GameObject image;

    //camera
    public Camera camera2;

    //if player is respawning
    public bool respawning = false;

    //if door is going right
    public bool right;

    public bool mid = false;
    
    void Start()
    {
        //makes sure this isn't destroyed between scenes
        DontDestroyOnLoad(transform.gameObject);

        //finds animator
        image = GameObject.Find("transitionImage");
        Atransition = image.GetComponent<Animator>();

        //gets the name of the scene
        Scene scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        //updates armor
        health = FindObjectOfType<Health>();

        //sets the transition to play the right animation
        Atransition.SetBool("right", right);

        if (currentScene == "room 1 enter")
        {
            StartCoroutine(loadWait());
        }
        else
        {
            Atransition.SetBool("start_new", true);
        }
    }

    //waits for lag
    IEnumerator loadWait()
    {
        yield return new WaitForSeconds(1f);
        Atransition.SetBool("start_new", true);
    }

    //loads main menu if quitting
    public void quitting()
    {
        sceneName = "start menu";
        LoadNext();
    }

    public void Continue()
    {
        Atransition.SetTrigger("mid");
        mid = true;
    }

    //passes data to player save script if saving
    public void saving()
    {
        //gets the name of the scene
        Scene scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        //makes the entrance room into the regular room
        if (currentScene == "room 1 enter")
        {
            currentScene = "room 1";
        }

        //gives name to saving script
        PlayerSave save = FindObjectOfType<PlayerSave>();
        save.level = currentScene;
    }

    public void LoadNext()
    {
        //starts the LoadLevel function
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        if (mid == false)
        {
            //starts animation
            Atransition.SetTrigger("start");
        }
        Atransition.SetTrigger("midn't");

        //makes sure animation doesn't continue
        Atransition.SetBool("start_new", false);

        //starts right animation
        Atransition.SetBool("right", right);

        //waits, then continues rest of code
        yield return new WaitForSeconds(transitionTime);

        //if player is respawning
        if (respawning)
        {
            health.alive = true;
        }

        //if player is going to start menu
        if (sceneName == "start menu")
        {
            //destroys camera and player
            Destroy(player);
            CameraController cc = FindObjectOfType<CameraController>();
            cc.destroy = true;

            //waits, then continues rest of code
            yield return new WaitForSeconds(0.1f);

            //loads next scene
            SceneManager.LoadScene(sceneName);

            //destroys UI
            Destroy(UI);

            //destroys this
            Destroy(gameObject);
        }
        else
        {
            //transforms the player
            player.transform.position = new Vector3(xPosition, yPosition, 0);

            //loads next scene
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            //waits, then continues rest of code
            yield return new WaitForSeconds(respawning ? 0.5f : 0.25f);

            //plays the right animation
            Atransition.SetBool("right", right);

            //starts next animation in next scene
            Atransition.SetBool("start_new", true);

            respawning = false;
        }
    }  
}
