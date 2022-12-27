using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    //player transform
    public Transform player;

    //offset between player and camera
    private Vector3 offset;

    //if the camera should be destroyed
    public bool destroy = false;

    //if camera can be destroyed upon load
    public bool destroyable = false;

    // The speed with which the camera will be following.           
    public float smoothing = 5f;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.position;

        //if camera will be destroyed on load
        if (!destroyable)
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Create a postion the camera is aiming for
            Vector3 targetCamPos = player.position + offset;

            // Smoothly interpolate camera position
            transform.position = Vector3.Lerp(transform.position,
                                               targetCamPos,
                                               smoothing * Time.deltaTime);
        }

        //if camera should be destroyed
        if (destroy)
        {
            Destroy(gameObject);
            destroy = false;
        }
    }
}