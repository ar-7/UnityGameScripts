using UnityEngine;

/*
 * Lines 89-108 Physics based movement calculations adapted from: https://www.youtube.com/watch?v=8-UBALp1xQc&t=1s
MIT License

Copyright (c) 2019 World of Zero

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

public class EnemyRigidbodyMovementController : MonoBehaviour
{
    // This script handles the physics based movement of the enemies and pathfinding to make the enemies avoid asteroids and reach the player

    public float velocity; // Speed of the enemy
    public float rotationSpeed; // Rate of turn
    public float horizontal; // input variable controlling left to right movement
    public float vertical; // input variable controlling up and down movement
    public Transform followedObject; // variable storing the player as target
    private new Rigidbody rigidbody; // variable for storing the rigidbody belonging to the object this script is attached to 

    public float enemyDetectionRange; // Static variable defining the detection range for each enemy

    float raycastPos = 1.5f; // Raycast position for the collision avoidance system
    float collisionDetectionDistance = 12f; // detection distance for the collision avoidance system

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); // Sets the rigidbody of the parent object

        // Making sure the enemies are on the same plane as the player. Shouldn't be needed, but just in case
        var currentPosition = transform.position;
        currentPosition.y = 0f;
        transform.position = currentPosition;

        FindPlayer(); // Gets the player as target

        // Add some randomness to the detection range, this creates variety among the enemies on each level. Large range means wider scope for the behaviour.
        enemyDetectionRange = Random.Range(15, 60);
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

    // Update is called once per frame
    void Update()
    {
        // Makes sure the player is set as target before doing anything
        if (followedObject == null)
        {
            FindPlayer();
        }
        else
        {
            // Wakes up the enemy when the player is within its detection range
            if (Vector3.Distance(transform.position, followedObject.position) < enemyDetectionRange)
            {
                Pathfinding(); // Starts moving
            }
        }
    }
    // Physics based movement calculations
    void Move()
    {
        var inputDirection = new Vector3(horizontal, 0, vertical); // Creates the vector coordinates for the direction of travel. y is zero because there is no verticality to the game world.
        var thrust = Vector3.Dot(inputDirection.normalized, this.transform.forward);
        this.rigidbody.AddForce(thrust * inputDirection.magnitude * this.transform.forward * velocity * Time.deltaTime); // Adds force to the physical rigidbody of the object
    }

    void Turn()
    {
        //Follower
        var difference = (followedObject.position - this.transform.position).normalized; // Calculates the difference between the player's position and the enemy's current position
        horizontal = difference.x; // sets the horizontal input to the x axis of the calculated difference
        vertical = difference.z; // sets the vertical input to the z axis of the calculated difference

        // Turn calculations
        var inputDirection = new Vector3(horizontal, 0, vertical);
        var rotation = Vector3.Dot(inputDirection.normalized, this.transform.right);
        var rotationAmount = rotationSpeed * Time.deltaTime * rotation;
        this.rigidbody.AddTorque(0, rotationAmount, 0); // Adds rotational force to the physical rigidbody of the object
    }

    // Collision avoidance system that calls the movement functions
    void Pathfinding()
    {
        RaycastHit hit; // Hit object for the raycasts
        Vector3 raycastOffset = Vector3.zero; // Create the offset for the raycasts

        Vector3 left = transform.position - transform.right * raycastPos; // left position for detection ray
        Vector3 right = transform.position + transform.right * raycastPos; // right position for detection ray

        // Visualises the rays in the editor
        Debug.DrawRay(left, transform.forward * collisionDetectionDistance, Color.red);
        Debug.DrawRay(right, transform.forward * collisionDetectionDistance, Color.red);

        // check if hits are detected, left first, then right
        if (Physics.Raycast(left, transform.forward, out hit, collisionDetectionDistance))
        {
            raycastOffset += Vector3.right; // Add one right to the vector, this is used below
        }
        else if (Physics.Raycast(right, transform.forward, out hit, collisionDetectionDistance))
        {
            raycastOffset -= Vector3.right; // Subtract one from the vector, this is used below
        }

        // if one has been added to or subtracted from a vector, making it not zero
        if (raycastOffset != Vector3.zero)
        {
            // Rotation calculations using the offset values to turn the enemy away from the object it is heading towards
            var rotation = Vector3.Dot((raycastOffset * 300f).normalized, this.transform.right);
            var rotationAmount = rotationSpeed * Time.deltaTime * rotation;
            this.rigidbody.AddTorque(0, rotationAmount, 0); // Applies the tourqe
        }
        else
        {
            Turn(); // if nothing is detected, keep on using the standard turn calculations
        }

        Vector3 front = transform.position + transform.forward * raycastPos; // little ray out in front to detect whether the enemy has nosed into an object

        Debug.DrawRay(front, transform.forward * 3f, Color.blue); // visual representation of the ray

        // check if hit is detected
        if (Physics.Raycast(front, transform.forward, out hit, 3f))
        {
            var inputDirection = new Vector3(horizontal, 0, -vertical); // flips the vertical axis
            var thrust = Vector3.Dot(inputDirection.normalized, this.transform.forward); 
            this.rigidbody.AddForce(thrust * inputDirection.magnitude * this.transform.forward * velocity * Time.deltaTime); // applies the flipped force
            this.rigidbody.AddTorque(0, 30, 0); // give the enemy a jolt of torque to push its nose off the object it is stuck on
        }
        else
        {
            Move(); // if nothing is detected, move as usual.
        }
    }

}
