using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data : MonoBehaviour
{
    public bool load = false;

    //if fog should be disabled
    public bool disableFog;

    //referencing an animator
    public Animator Atransition;

    void Start()
    {
        //makes sure this object isn't deleted when loading from the main menu
        DontDestroyOnLoad(transform.gameObject);
    }

    //tells script to wait
    public void setFog()
    {
        StartCoroutine(wait());
    }

    //delay
    IEnumerator wait()
    {
        //delay
        yield return new WaitForSeconds(0.5f);
        //if loading
        if (load)
        {
            loader();
        }
        Fogger();
    }

    public void Fogger()
    {
        //if fog should be disabled
        if (disableFog)
        {
            test test = FindObjectOfType<test>();
            test.Disable();
        }
        //destroys this
        Destroy(gameObject);
    }

    void loader()
    {
        //tells transition animator to play a black screen until save is done loading
        transition transition = FindObjectOfType<transition>();
        transition.Continue();

        //tells save script to load save file
        PlayerSave save = FindObjectOfType<PlayerSave>();
        save.Loading();
        //sets load to default false
        load = false;
        //destroys this save object after it is done saving
        Destroy(gameObject);
    }
}
