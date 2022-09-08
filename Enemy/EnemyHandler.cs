using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{
    // This script handles the various properties and abilities of the enemy ships

    public int damage; // Var for storing weapon damage amount
    public float range; // Var for storing range of weapon attack
    public float fireRate; // Var for storing weapon rate of fire
    private float nextTimeToFire = 0f; // Var for storing delay time for fire rate calculation
    public ParticleSystem attackExplo; // Particle object for weapon effect 
    GameObject player; // Player object
    public GameObject explosion; // The effect that plays when the enemy dies
    public float health = 50f; //  // Var for storing health value of enemy
    public int points = 50; //  // Var for storing points earned on destruction 

    // Start is called before the first frame update
    void Start()
    {
        FindPlayer(); // Make sure the player is stored as target
    }
    // Function that finds the player and clears errors if the player is not findable
    void FindPlayer()
    {
        // Setting the player as the follow target
        if (player == null)
        {
            GameObject temp = GameObject.Find("Player");

            if (temp != null)
            {
                player = temp;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If the player is not set find player
        if (player == null)
        {
            FindPlayer();
        }
        else // If the player is set 
        {
            // Detect distance to player and time until weapon is ready to fire
            if (Vector3.Distance(transform.position, player.transform.position) < range && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Attack(); // Fire
            }
        }
    }

    void Attack()
    {
        // Play effect
        attackExplo.Play();
        // Play shooting sound
        FindObjectOfType<AudioManager>().Play("EnemyAttack");

        // Get player object to call public function
        PlayerHandler targetPlayer = player.GetComponent<PlayerHandler>();
        if (targetPlayer != null)
        {
            targetPlayer.TakeDamage(damage); // feed damage amount to player's take damage function
        }
    }

    // Function that receives an amount of damage
    public void TakeDamage(float amount)
    {
        health -= amount; // Reduce health by the amount of damage received

        // Call the die function if health is zero or less
        if (health <= 0f)
        {
            Die();
        }
    }

    // Function called by the death event
    void Die()
    {
        // Create instance of effect
        GameObject temp = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        //Play explosion sound
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        // Destroy enemy
        Destroy(gameObject);
        // Signal death event
        EventManager.EnemyDeath();
        //Reward player for the kill
        EventManager.ScorePoints(points);
        // Clean up effect
        Destroy(temp, 5f);
    }    
}
