using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slots : MonoBehaviour
{
    //the current item in the slot
    public string item;

    //the image component
    public Image im;

    //the slot component
    public int slot;

    //if object is over armor slot
    void OnTriggerEnter2D(Collider2D other)
    {
        //checks what object is over it
        switch (other.gameObject.name)
        {
            case "grass":
                setItem("grass");
                im.color = new Color(27f / 255, 154f / 255, 38f / 255, 1);
                break;

            case "wood":
                setItem("wood");
                im.color = new Color(137f / 255, 49f / 255, 20f / 255, 1);
                break;

            case "spines":
                setItem("spines");
                im.color = new Color(140f / 255, 140f / 255, 140f / 255, 1);
                break;

            case "titanium":
                setItem("titanium");
                im.color = new Color(89f / 255, 89f / 255, 89f / 255, 1);
                break;

            case "mud":
                setItem("mud");
                im.color = new Color(84f / 255, 28f / 255, 0, 1);
                break;

            case "bandage":
                setItem("bandage");
                im.color = new Color(1, 1, 1, 1);
                break;

            case "dracoSkin":
                setItem("dracoSkin");
                im.color = new Color(1, 156f / 255, 0, 1);
                break;
        }
    }

    //sets what items are in the slots upon loading
    public void setting()
    {
        //checks what object is over it
        switch (item)
        {
            case "grass":
                setItem("grass");
                im.color = new Color(27f / 255, 154f / 255, 38f / 255, 1);
                break;

            case "wood":
                setItem("wood");
                im.color = new Color(137f / 255, 49f / 255, 20f / 255, 1);
                break;

            case "spines":
                setItem("spines");
                im.color = new Color(140f / 255, 140f / 255, 140f / 255, 1);
                break;

            case "titanium":
                setItem("titanium");
                im.color = new Color(89f / 255, 89f / 255, 89f / 255, 1);
                break;

            case "mud":
                setItem("mud");
                im.color = new Color(84f / 255, 28f / 255, 0, 1);
                break;

            case "bandage":
                setItem("bandage");
                im.color = new Color(1, 1, 1, 1);
                break;

            case "dracoSkin":
                setItem("dracoSkin");
                im.color = new Color(1, 156f / 255, 0, 1);
                break;
        }
    }

    //tells inventory what item is in slot
    void setItem(string name)
    {
        if (item != name)
        {
            inventory iv = FindObjectOfType<inventory>();
            iv.itemNames[slot] = name;

            item = name;
        }        
    }
}
