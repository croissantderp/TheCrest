using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quartz : MonoBehaviour
{
    //metronome slider
    public Slider slider;

    //sets max time on metronome
    public void SetMaxTime(float time)
    {
        slider.maxValue = time;
        slider.value = time;
    }

    //sets current time on metronome
    public void SetTime(float time)
    {
        slider.value = time;
    }
}
