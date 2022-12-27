using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class beam : MonoBehaviour
{
    //components
    public Transform Transform;
    public Collider2D area;
    public Transform bladeObject;
    public MeshRenderer render;
    public Material material1;
    public Material material2;
    public enemySpawner es;

    //camera component
    public Camera Camera;

    //the bpm
    [HideInInspector]
    public float bpm;
    [HideInInspector]
    public bool beat;

    //if blade is waiting/recharging
    bool waiting;

    //if the blade can swing
    bool canSwing = true;

    public bool infinite = false;

    //duration of blade remaining (used for fractional notes)
    public float dureRemain = 1;

    Vector2 mousePos;

    void Start()
    {
        bpm = es.bpm;
    }

    // Update is called once per frame
    void Update()
    {
        //faces mouse
        faceMouse();

        //lets player use blade on beats
        if (beat)
        {
            //recharges blade on the beat
            canSwing = true;
            dureRemain = 1;

            //enables blade
            area.enabled = true;
            render.material = material2;

            //sets waiting to false
            waiting = false;
            beat = false;
        }

        if (infinite)
        {
            canSwing = true;
        }

        //disables blade
        if (!canSwing && !waiting)
        {
            StartCoroutine(wait());
            waiting = true;
        }

        //deactivates blade once duration is used up
        if (dureRemain <= 0)
        {
            canSwing = false;
        }
    }

    //recharge
    IEnumerator wait()
    {
        yield return new WaitForSeconds(bpm / 6000);

        //sets recharging to true
        es.recharging = true;

        //disables blade
        area.enabled = false;
        render.material = material1;

    }

    //gets mouse position
    public void GetMousePos(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    //faces mouse
    void faceMouse()
    {
        //sends a ray towards the mouse on a plane
        Ray cameraRay = Camera.ScreenPointToRay(mousePos);
        Plane plane = new Plane(Vector3.forward, Vector3.up);
        float rayLength;

        //if ray hits anything
        if (plane.Raycast(cameraRay, out rayLength))
        {
            //points ray
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            //shows the ray in debug
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            //gets the direction to look in
            Vector3 direction = pointToLook - Transform.position;

            //sets transform.up to that direction
            Transform.up = direction;
        }
    }
}
