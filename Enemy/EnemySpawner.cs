using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // This script distributes the inital enemies and handles respawns for the level

    [SerializeField] GameObject enemyPrefab; // Prefab for enemy
    [SerializeField] int numberOfEnemiesOnAxis = 7; // Number of asteroids to spawn on each axis
    [SerializeField] int enemyGridSpacing = 65; // Spacing of the asteroid grid
    public List<GameObject> emptyGrid = new List<GameObject>(); // List for the empty grid
    public List<GameObject> validLocations = new List<GameObject>(); // List that stores the locations which have no collisions. These become the enemy spawns.
    [SerializeField] GameObject player; // Stores the player as target

    // Starts listening for events
    private void OnEnable()
    {
        EventManager.onStartGame += FirstDistribution; // Defines function to run when the event happens
        EventManager.onEnemyDeath += GenerateNewEnemies;
    }

    // stops listening for events
    private void OnDisable()
    {
        EventManager.onStartGame -= FirstDistribution;
        EventManager.onEnemyDeath -= GenerateNewEnemies;
    }
    // Start is called before the first frame update
    void Start()
    {
        FindPlayer(); // Make sure the player is set as target
    }
    void FindPlayer()
    {
        // Setting the player as target, this is used when making sure an enemy will not spawn too close to the player 
        if (player == null)
        {
            GameObject temp = GameObject.Find("Player");

            if (temp != null)
            {
                player = temp;
            }
        }
    }
    // function for the initialisation of the grid and first spawns
    void FirstDistribution()
    {
        PlaceEnemies(emptyGrid); // Feeding the list to populate into the instantiation loops
    }

    void PlaceEnemies(List<GameObject> locations)
    {
        // Loops through each axis to generate the base coordinates for each asteroid. Esentially creates a grid.
        for (int x = 0; x < numberOfEnemiesOnAxis; x++)
        {
            for (int z = 0; z < numberOfEnemiesOnAxis; z++)
            {
                InstantiateEnemyGrid(-x, 0, z);
            }
        }

        // Check each location for existing object
        foreach (GameObject reservedSpace in locations)
        {
            // Look for asteroid collisions.
            Collider[] collisions = Physics.OverlapSphere(reservedSpace.transform.position, 4f);
            // if there is no collision
            if (collisions.Length == 0 && Vector3.Distance(reservedSpace.transform.position, player.transform.position) > 15f)
            {
                // Instantiate enemy at location
                Instantiate(enemyPrefab, reservedSpace.transform.position, Quaternion.identity, transform);
                validLocations.Add(reservedSpace); // Create the list of valid spaces that can be used to replenish the enemies
            }
            else // if there is a collision
            {
                Destroy(reservedSpace); // discard this empty object
            }
        }
        emptyGrid.Clear(); // Clear the emptyGrid array as it is no longer needed.
    }

    // Creates the initial empty grid of enemies
    void InstantiateEnemyGrid(int x, int y, int z)
    {
        GameObject temp = new GameObject("empty"); // Create placeholders to populate the new grid
        // Generate an obj with position in the scene, saved in a temp var
        GameObject emptyObj = Instantiate(temp, new Vector3(transform.position.x + (x * enemyGridSpacing) + EnemyOffset(), transform.position.y + (-y * enemyGridSpacing), transform.position.z + (z * enemyGridSpacing) + EnemyOffset()), Quaternion.identity, transform) as GameObject;
        // Append the empty obj to the grid list
        emptyGrid.Add(emptyObj);
        Destroy(temp); // Clean up the temp objects
    }

    // generates random numbers to use for offsetting the coordinates of the asteroids.
    float EnemyOffset()
    {
        return Random.Range(-enemyGridSpacing / 1.6f, enemyGridSpacing / 1.6f);
    }

    // Function for respawning the enemies during gameplay
    void GenerateNewEnemies()
    {
        // Check how many enemies exist against how many available locations

        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find how many current enemies there are

        // Check how many enemies exist against how many available locations
        if ((validLocations.Count - currentEnemies.Length) >= Random.Range(3, 7)) // Add some randomness to how many enemies are collected before they respawn
        {
            Debug.Log("Spawning new enemies");
            foreach (GameObject availableSpace in validLocations) // Loop through the list
            {
                // Look for asteroid collisions.
                Collider[] collisions = Physics.OverlapSphere(availableSpace.transform.position, 4f);

                // if there is no collision
                if (collisions.Length == 0 && Vector3.Distance(availableSpace.transform.position, player.transform.position) > 15f)
                {
                    // Instantiate a new enemy at location
                    Instantiate(enemyPrefab, availableSpace.transform.position, Quaternion.identity, transform);
                }
            }
        }
    }
}
