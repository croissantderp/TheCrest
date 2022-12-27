using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleLose : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //loads save file if lost
    public void loseLoad()
    {
        StartCoroutine(loseLoad2());
    }

    public void destroyThis()
    {
        Destroy(gameObject);
    }

    //loading function
    IEnumerator loseLoad2()
    {
        //delay
        yield return new WaitForSeconds(1f);

        //transition
        transition tn = FindObjectOfType<transition>();
        tn.Continue();

        //loads this
        PlayerSave save = FindObjectOfType<PlayerSave>();
        save.Loading();

        //then destroys this
        Destroy(gameObject);
    }
}
