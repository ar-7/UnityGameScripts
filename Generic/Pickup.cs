using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // This script handles the behaviours of pickups

    [SerializeField] float rotationOffset = 20f; // sets a base rotation speed
    Vector3 randomRotation; // var for offsetting the rotation
    public int points = 100; // Points rewarded for collecting the pickup
    [SerializeField] GameObject explosion; // The effect that plays when the object dies
    public GameObject TimeBonusForKill; // The pop-up that lets the player know they received extra time

    // Start is called before the first frame update
    void Start()
    {
        //Generate random rotation values
        randomRotation.x = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.y = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.z = Random.Range(-rotationOffset, rotationOffset);
    }

    // Update is called once per frame
    void Update()
    {
        // Apply rotatation values to the object
        gameObject.transform.Rotate(randomRotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider coll)
    {
        // If the player enters the collider
        if (coll.transform.CompareTag("Player"))
        {
            PickedUp(); // trigger the collection
        }
    }

    public void PickedUp()
    {
        FindObjectOfType<AudioManager>().Play("Pickup"); // Play sound
        gameObject.GetComponent<Animation>().Play(); // Activate the crate open animation
        EventManager.ScorePoints(points); // Add points for the pickup.
        gameObject.tag = "Untagged"; // Remove the tag to stop it being counted by the respawn script
        Invoke("DestroyPickup", 2f); // Waits two seconds for the animation to play before destroying the object
        EventManager.SpawnNewPickup(); // Calls the event to spawn new pickups
        PlayerRewards(); // Rewards the player with health and additional time
    }
    // Destroy picked up object with effect
    void DestroyPickup()
    {       
        Destroy(gameObject); // Waits two seconds before destroying pickup
        GameObject temp = Instantiate(explosion, transform.position, Quaternion.identity); // Play destruction effect
        Destroy(temp, 5f); // Clean up effect
    }
    void PlayerRewards()
    {
        // Add player health
        PlayerHandler player = GameObject.Find("Player").GetComponent<PlayerHandler>();
        if (player != null)
        {
            player.GainHealth(20); // Give player bonus health
        }
    }

}
