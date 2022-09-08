using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // This script causes the camera to follow the player

    public Transform target; // Stores the target, in this case it will be the player

    public float smoothSpeed = 0.15f; // speed of the easing at the end of the smooth movement

    public Vector3 offset; // Distance the camera should be from the player position 
    private Vector3 velocity = Vector3.zero; // storage for the velocity of the camera

    void LateUpdate()
    {
        // Sets the player as the target if it is not set already
        // Only runs the follow functionality if the target is set 
        if (target == null)
        {
            GameObject temp = GameObject.Find("Player");

            if (temp != null)
            {
                target = temp.transform;
            }
        }
        else
        {
            Vector3 desiredPosition = target.position + offset; // offsets teh camera from the player position

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed); // Smoothly moves the camera toward the desired position
        }
    }
}
