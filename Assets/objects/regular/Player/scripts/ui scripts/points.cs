using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class points : MonoBehaviour
{
    //sliders
    public Slider slider;

    public Slider flats;

    public Slider sharps;

    //changes the value
    public void Change(int point, int flat, int sharp)
    {
        //changes values
        slider.value = point;
        flats.value = flat;
        sharps.value = sharp;
    }
}
