using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    //if the player has items
    public bool[] items = { false, false, false, false, false, false, false };

    //the type of items currently in slots
    public string[] itemNames = { };

    //gameobject array
    public GameObject[] itemObjects;

    //slot array for inventory slots
    public slots[] slot;

    //updates items
    public void UpdateItems()
    {
        //for loop that checks the different items
        for (int i = 0; i < 7; i++)
        {
            //if the player has that item
            if (items[i] == true)
            {
                //moves the items down if the player has them
                Vector3 Scale = itemObjects[i].transform.localPosition;
                Scale.y = 225;
                itemObjects[i].transform.localPosition = Scale;
            }
            else
            {
                //moves them up otherwise
                Vector3 Scale = itemObjects[i].transform.localPosition;
                Scale.y = 2000;
                itemObjects[i].transform.localPosition = Scale;
            }
        }
    }

    public void UpdateSlots()
    {
        //for loop that checks the different items
        for (int i = 0; i < 6; i++)
        {
            slot[i].item = itemNames[i];
            slot[i].setting();
        }
    }

    //updates armor
    public void UpdateArmor()
    {
        Health health = FindObjectOfType<Health>();
        health.itemNames = itemNames;
        health.items = items;

        health.SetArmor();
    }
}
