using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class deathScreen : MonoBehaviour
{
    //game over screen
    public GameObject dieScreen;

    //respawn button
    public Button respawnButton;

    //if player is alive
    public bool isAlive = true;

    void Start()
    {
        //moving the ui out of the screen range
        Vector3 Scale = transform.localPosition;
        Scale.y = 2000;
        transform.localPosition = Scale;

        //getting button component
        Button btn = respawnButton.GetComponent<Button>();
		btn.onClick.AddListener(respawn);
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is still alive and also shows death screen
        if (!isAlive)
        {
            //moves ui into it's position
            Vector3 Scale = transform.localPosition;
            Scale.y = 0;
            transform.localPosition = Scale;
            //unlocks cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            //moves the ui out of screen range again
            Vector3 Scale = transform.localPosition;
            Scale.y = 2000;
            transform.localPosition = Scale;
        }
    }

    //reloads the scene
    public void respawn()
    {
        PlayerSave save = FindObjectOfType<PlayerSave>();
        save.respawn = true;
        save.Loading();
    }
}
