using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class save : MonoBehaviour
{
    public Button Load;
    public Button Save;
    public Button Quit;
    public Button QuitS;
    public Button QuitN;

    public GameObject player;
    public GameObject confirm;
    public Animator am;

    public inventory iv;

    public bool canClose = false;

    // Start is called before the first frame update
    void Start()
    {
        //finding the player
        player = GameObject.Find("player");

        //moving the ui out of the screen range
        //Vector3 Scale = transform.localPosition;
        //Scale.y = 2000;
        //transform.localPosition = Scale;

        Vector3 Scale2 = confirm.transform.localPosition;
        Scale2.y = 1000;
        confirm.transform.localPosition = Scale2;

        //getting the button component
        Button btn = Load.GetComponent<Button>();
        btn.onClick.AddListener(Load2);
        Button btn2 = Save.GetComponent<Button>();
        btn2.onClick.AddListener(Save2);
        Button btn3 = Quit.GetComponent<Button>();
        btn3.onClick.AddListener(Quit2);
        Button btn4 = QuitS.GetComponent<Button>();
        btn4.onClick.AddListener(QuitSave);
        Button btn5 = QuitN.GetComponent<Button>();
        btn5.onClick.AddListener(QuitNo);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("escape") && !canClose)
        {
            //transforms menu down
            //Vector3 Scale = transform.localPosition;
            //Scale.y = 0;
            //transform.localPosition = Scale;
            am.SetTrigger("enter");

            iv.UpdateItems();
            iv.UpdateSlots();

            //enables cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            canClose = true;            
        }
        else if (Input.GetKeyDown("escape"))
        {
            am.SetTrigger("exit");

            //transforms menu back up
            //Vector3 Scale = transform.localPosition;
            //Scale.y = 2000;
            //transform.localPosition = Scale;

            Vector3 Scale2 = confirm.transform.localPosition;
            Scale2.y = 1000;
            confirm.transform.localPosition = Scale2;

            iv.UpdateArmor();

            Health health = FindObjectOfType<Health>();
            health.SetArmor();

            //disables cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            canClose = false;
        }
    }

    //tells player save script to save
    void Save2()
    {
        iv.UpdateArmor();

        transition transition = FindObjectOfType<transition>();
        transition.saving();

        Health health = FindObjectOfType<Health>();
        health.Saving();

        PlayerSave save = FindObjectOfType<PlayerSave>();
        save.SavePlayer();
    }

    //tells player save script to load
    void Load2()
    {
        string path = Application.persistentDataPath + "/ge.cko";
        if (File.Exists(path))
        {
            PlayerSave save = FindObjectOfType<PlayerSave>();
            save.Loading();
        }    
    }

    //tells transition script to quit
    void Quit2()
    {
        Vector3 Scale = confirm.transform.localPosition;
        Scale.y = 0;
        confirm.transform.localPosition = Scale;
    }

    //if the player wants to save before quitting
    void QuitSave()
    {
        Save2();

        transition transition = FindObjectOfType<transition>();
        transition.quitting();
        transition.LoadNext();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //if the player does not want to save before quitting
    void QuitNo()
    {
        transition transition = FindObjectOfType<transition>();
        transition.saving();
        transition.quitting();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
