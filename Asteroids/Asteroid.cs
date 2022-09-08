using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // This script defines the properties of the asteroid it is attached to.

    // Minimum and maximum modification to the scale of each asteroid
    [SerializeField] float minScale = .9f; 
    [SerializeField] float maxScale = 1.8f;

    [SerializeField] float rotationOffset = 20f; // Speed of rotation
    
    Transform asteroidTransform; // Var to cache the transform of the current asteroid
    Vector3 randomRotation; // Coordinates to be used in the rotation of the asteroids

    // Start is called before the first frame update
    private void Start()
    {
        asteroidTransform = transform; // getting the transform of the object this script is attached to 

        // generating random size variables in indvidual coordinates
        Vector3 scale = Vector3.one;
        scale.x = Random.Range(minScale, maxScale);
        scale.y = Random.Range(minScale, maxScale);
        scale.z = Random.Range(minScale, maxScale);

        asteroidTransform.localScale = scale; // setting the asteroid's scale to the coordinates calculated above

        // generating random rotation coordinates
        randomRotation.x = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.y = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.z = Random.Range(-rotationOffset, rotationOffset);
    }

    // Update is called once per frame
    void Update()
    {
        asteroidTransform.Rotate(randomRotation * Time.deltaTime); // Setting random rotation coordinates
    }
}
