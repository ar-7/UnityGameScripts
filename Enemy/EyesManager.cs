using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesManager : MonoBehaviour
{
    // This function is solely for making the spooky eyes of the enemies turn on when the enemy wakes up
    // This important feature gets its own script because it needs to activate on a per enemy basis, it would be pointless to light all the eyes up at once
    // It is intended to be applied to an empty container holding the eyes objects, that way all children can be modified without worrying about affecting anything else in the prefab

    public Transform followedObject; // Var for storing the player object as target
    private float activateRange; // Var for keeping the detection range in

    // Start is called before the first frame update
    void Start()
    {
        FindPlayer(); // Makes sure the player is set as target
        activateRange = GetComponentInParent<EnemyRigidbodyMovementController>().enemyDetectionRange; // Sets the detection range of the eyes based on the detection range of the parent enemy
    }
    void FindPlayer()
    {
        // Setting the player as the follow target
        if (followedObject == null)
        {
            GameObject temp = GameObject.Find("Player");

            if (temp != null)
            {
                followedObject = temp.transform;
            }
        }
    }
    void LateUpdate()
    {
        // if statement that makes sure the script only attempts to run if the player is alive and set as target
        if (followedObject == null)
        {
            FindPlayer();
        }
        else
        {
            if (Vector3.Distance(transform.position, followedObject.transform.position) < activateRange) // if the player is close enough to wake up the enemy
            {
                foreach (MeshRenderer x in gameObject.GetComponentsInChildren<MeshRenderer>()) // Finds the renderer for the eyes
                {
                    x.enabled = true; // turn the renderer on
                }
            }
            else
            {
                foreach (MeshRenderer x in gameObject.GetComponentsInChildren<MeshRenderer>())
                {
                    x.enabled = false; // turn the renderer off
                }
            }
        }
    }
}
