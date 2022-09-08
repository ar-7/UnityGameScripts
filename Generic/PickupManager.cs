using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    // This script distributes the pickups for the level

    [SerializeField] GameObject pickupPrefab; // Prefab for pickup
    [SerializeField] int numberOfPickupsOnAxis = 4; // Number of asteroids to spawn on each axis
    [SerializeField] int pickupGridSpacing = 85; // Spacing of the asteroid grid
    public List<GameObject> emptyGrid = new List<GameObject>(); // List for the empty grid
    public List<GameObject> validLocations = new List<GameObject>(); // List that stores the locations which have no collisions. These become the pickup spawns.

    // Starts listening for events
    private void OnEnable()
    {
        EventManager.onStartGame += FirstDistribution;
        EventManager.onSpawnNewPickup += GenerateNewPickups;
    }

    // stops listening for events
    private void OnDisable()
    {
        EventManager.onStartGame -= FirstDistribution;
        EventManager.onSpawnNewPickup -= GenerateNewPickups;
    }

    void FirstDistribution()
    {
        PlacePickups(emptyGrid); // Feeding the list to populate into the instantiation loops
    }

    void PlacePickups(List<GameObject> locations)
    {
        // Loops through each axis to generate the base coordinates for each asteroid. Esentially creates a grid.
        for (int x = 0; x < numberOfPickupsOnAxis; x++)
        {
            for (int z = 0; z < numberOfPickupsOnAxis; z++)
            {
                InstantiatePickupGrid(x, 0, z);
            }
        }

        // Check each location for existing object
        foreach (GameObject reservedSpace in locations)
        {
            // Look for asteroid collisions.
            Collider[] collisions = Physics.OverlapSphere(reservedSpace.transform.position, 4f);

            // if there is no collision
            if (collisions.Length == 0)
            {
                // Instantiate crate at location
                Instantiate(pickupPrefab, reservedSpace.transform.position, Quaternion.identity, transform);
                validLocations.Add(reservedSpace); // Create the list of valid spaces that can be used to replenish the pickups
            }
            else // if there is a collision
            {
                Destroy(reservedSpace); // discard this empty object
            }
        }
        emptyGrid.Clear(); // Clear the emptyGrid array as it is no longer needed.
    }

    // Creates the initial empty grid of pickups
    void InstantiatePickupGrid(int x, int y, int z)
    {
        GameObject temp = new GameObject("empty"); // Create placeholders to populate the new grid
        // Generate an obj with position in the scene, saved in a temp var
        GameObject emptyObj = Instantiate(temp, new Vector3(transform.position.x + (x * pickupGridSpacing) + PickupOffset(), transform.position.y + (-y * pickupGridSpacing), transform.position.z + (z * pickupGridSpacing) + PickupOffset()), Quaternion.identity, transform) as GameObject;
        // Append the empty obj to the grid list
        emptyGrid.Add(emptyObj);
        Destroy(temp); // Clean up the temp objects
    }

    // generates random numbers to use for offsetting the coordinates of the asteroids.
    float PickupOffset()
    {
        return Random.Range(-pickupGridSpacing / 1.8f, pickupGridSpacing / 1.8f);
    }
    
    // Function for respawning the pickups during gameplay
    void GenerateNewPickups()
    {
        // Check how many pickups exist against how many available locations

        GameObject[] currentPickups = GameObject.FindGameObjectsWithTag("Pickup"); // Find how many current pickups there are

        // Check how many pickups exist against how many available locations
        if ((validLocations.Count - currentPickups.Length) >= Random.Range(4, 10)) // Add some randomness to how many pickups are collected before they respawn
        {
            foreach (GameObject availableSpace in validLocations) // Loop through the list
            {
                // Look for asteroid collisions.
                Collider[] collisions = Physics.OverlapSphere(availableSpace.transform.position, 3f);

                // if there is no collision
                if (collisions.Length == 0)
                {
                    // Instantiate a new pickup at location
                    Instantiate(pickupPrefab, availableSpace.transform.position, Quaternion.identity, transform);
                }
            }
        }
    }
}
