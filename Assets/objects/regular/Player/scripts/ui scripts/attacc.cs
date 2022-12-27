using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacc : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //calls menu if x is pressed
        if (Input.GetKey("x"))
        {
            //moving the ui out of the screen range
            Vector3 Scale = transform.localPosition;
            Scale.y = 400;
            transform.localPosition = Scale;
        }
        else
        {
            //moving the ui out of the screen range
            Vector3 Scale = transform.localPosition;
            Scale.y = 2000;
            transform.localPosition = Scale;
        }
    }
}
