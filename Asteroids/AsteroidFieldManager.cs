using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldManager : MonoBehaviour
{
    // This script generates an asteroid field

    // Declaring some vars to contain the asteroid and settings
    [SerializeField] Asteroid asteroidPrefab; // Prefab for asteroid
    [SerializeField] int numberOfAsteroidsOnAxis = 23; // Number of asteroids to spawn on each axis
    [SerializeField] int asteroidGridSpacing = 23; // Spacing of the asteroid grid

    // Prepares a list to contain all asteroid objects
    public List<Asteroid> asteroid = new List<Asteroid>();

    private void Start()
    {
        // Place the asteroids to pre-load the environment.
        PlaceAsteroids();
    }

    void PlaceAsteroids()
    {
        // Loops through each axis to generate the base coordinates for each asteroid. Essentially creates a grid.
        for (int x = 0; x < numberOfAsteroidsOnAxis; x++)
        {
            for (int z = 0; z < numberOfAsteroidsOnAxis; z++)
            {
                InstantiateAsteroid(x, 0, z);
            }
        }
    }

    // Generates the instances of the asteroids.
    void InstantiateAsteroid(int x, int y, int z)
    {
        // Generate an asteroid with position in the scene, saved in a temp var
        Asteroid temp = Instantiate(asteroidPrefab, new Vector3(transform.position.x + (x * asteroidGridSpacing) + AsteroidOffset(), transform.position.y + (-y * asteroidGridSpacing), transform.position.z + (z * asteroidGridSpacing) + AsteroidOffset()), Quaternion.identity, transform) as Asteroid;
        // Append the temp asteroid to the asteroid list
        asteroid.Add(temp);
    }

    // generates random numbers to use for offsetting the coordinates of the asteroids.
    float AsteroidOffset()
    {
        return Random.Range(-asteroidGridSpacing / 2f, asteroidGridSpacing / 2f);
    }
}
