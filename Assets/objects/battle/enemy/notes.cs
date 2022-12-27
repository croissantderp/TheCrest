using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//a script to store the information for a song
public class notes : MonoBehaviour
{
    public string title = "never gonna give you up";
    public float bpm = 180;
    public string time = "4/4";
    public string[] note = { "d", "a" };
    public float[] length = { 1, 2, };
    public float[] duration = { 1, 2 };
    public AudioClip audioClip;
    public float audioDelay;
}
