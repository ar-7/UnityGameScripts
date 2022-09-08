using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_AsteroidFieldManager : MonoBehaviour
{
    // This script generates an asteroid field

    // Declaring some vars to contain the asteroid and settings
    [SerializeField] Asteroid asteroid;
    [SerializeField] int numberOfAsteroidsOnAxis = 20;
    [SerializeField] int gridSpacing = 50;


    // Start is called before the first frame update
    void Start()
    {
        // Place the asteroids to pre-load the background.
        PlaceAsteroids();
    }

    void PlaceAsteroids()
    {
        // Loops through each axis to generate the base coordinates for each asteroid. Essentially creates a grid.
        for (int x = 0; x < numberOfAsteroidsOnAxis; x++)
        {
            for (int y = 0; y < numberOfAsteroidsOnAxis; y++)
            {
                for (int z = 0; z < numberOfAsteroidsOnAxis; z++)
                {
                    InstantiateAsteroid(x, y, z);
                }
            }
        }
    }

    // generates random numbers to use for offsetting the coordinates of the asteroids.
    float AsteroidOffset()
    {
        return Random.Range(-gridSpacing / 2f, gridSpacing / 2f);
    }

    // Generates the instances of the asteroids.
    void InstantiateAsteroid(int x, int y, int z)
    {
        Instantiate(asteroid, new Vector3(transform.position.x + (x * gridSpacing) + AsteroidOffset(), transform.position.y + (-y * (gridSpacing)) + AsteroidOffset(), transform.position.z + (z * gridSpacing) + AsteroidOffset()), Quaternion.identity, transform);
    }
}
