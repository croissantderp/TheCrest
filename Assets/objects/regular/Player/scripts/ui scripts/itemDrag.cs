using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //this object
    public GameObject Object;

    //x and y position
    public float xPos;
    public float yPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //empty
    }

    public void OnDrag(PointerEventData eventData)
    {
        //moves item with mouse
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //transforms item back when released
        Vector3 Scale = transform.localPosition;
        Scale.y = yPos;
        Scale.x = xPos;
        transform.localPosition = Scale;

    }
}
