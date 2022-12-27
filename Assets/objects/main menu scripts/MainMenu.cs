using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.IO;

public class MainMenu : MonoBehaviour
{
    //referencing an animator
    public Animator Atransition;

    //the time LoadLevel waits
    public float transitionTime = 1f;

    //the next scene name
    public string Scene;

    //if the game is starting
    public bool starting;

    public AudioMixer audioMixer;

    void Start()
    {
        //makes cursor visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //skips wait
        Atransition.SetBool("start_new", true);
        Scene = "room 1 enter";

        //retrieves settings and applys them
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("volume", -80));
     
        if (PlayerPrefs.GetInt("fullscreen", 0) == 0)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }

        data data = FindObjectOfType<data>();
        if (PlayerPrefs.GetInt("fog", 0) == 0)
        {
            data.disableFog = false;
        }
        else
        {
            data.disableFog = true;
        }
    }

    //press start
    public void PlayGame()
    {
        starting = true;
        LoadNext();
    }

    //press options
    public void Options()
    {
        Scene = "options";
        LoadNext();
    }

    //press quit
    public void QuitGame()
    {
        Application.Quit();
    }

    //press resume
    public void ResumeGame()
    {
        //game is starting
        starting = true;

        //if there is a save file
        string path = Application.persistentDataPath + "/ge.cko";
        if (File.Exists(path))
        {
            //tells data it's loading
            data data = FindObjectOfType<data>();
            data.load = true;
            LoadNext();
        }       
    }

    public void LoadNext()
    {
        //starts the LoadLevel function
        StartCoroutine(LoadLevel());
    }

    //loading function
    IEnumerator LoadLevel()
    {
        //starts animation
        Atransition.SetTrigger("start");
        Atransition.SetBool("start_new", false);

        //waits, then continues rest of code
        yield return new WaitForSeconds(transitionTime);

        //tells data to set fog
        if (starting)
        {
            data data = FindObjectOfType<data>();
            data.setFog();
        }

        //loads next scene
        SceneManager.LoadScene(Scene);
    }
}
