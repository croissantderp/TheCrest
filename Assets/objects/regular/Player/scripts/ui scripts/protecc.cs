using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class protecc : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //calls menu if z is pressed
        if (Input.GetKey("z"))
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
