using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

//THIS SCRIPT IS FOR TURNING OFF FOG, DO NOT DELETE!
public class test : MonoBehaviour
{
    //hd components
    public Volume volume;
    public Fog fog;

    // Start is called before the first frame update
    void Start()
    {
        //finds fog component on global volume
       if (volume.sharedProfile.TryGet<Fog>(out Fog temp))
       {
            //makes fog the profile
            fog = temp;
       }
    }

    // disables fog
    public void Disable()
    {
        //disables the fog
        fog.enabled.value = false;
    }
}
