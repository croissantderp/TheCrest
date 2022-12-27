using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //makes sure canvas isn't destroyed on load
        DontDestroyOnLoad(transform.gameObject);
    }
}
