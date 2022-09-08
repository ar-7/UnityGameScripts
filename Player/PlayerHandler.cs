using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // This script handles the gameplay properties of the player, health and death

    public int maxHealth = 100; // Max health is stored here, default value can be changed in the editor
    public int currentHealth; // Var available for keeping track of player health
    public HealthBar healthBar; // UI object for health display
    [SerializeField] GameObject explosion; // The effect that plays when the player dies

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("AmbientHum"); // Play ambient sound
        // Set the available health for the player
        currentHealth = maxHealth;
        // Send the max health amount to the UI health bar
        healthBar.SetMaxHealth(maxHealth);
    }
    // Function for taking damage, receives the amount as an argument
    public void TakeDamage(int amount)
    {
        // Remove damage amount from hp pool
        currentHealth -= amount;
        // Update visual display of health amount
        healthBar.SetHealth(currentHealth);
        // When health reaches zero runs death function
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    public void GainHealth(int amount)
    {
        // Add amount to hp pool
        currentHealth += amount;
        if (currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
        // Update visual display of health amount
        healthBar.SetHealth(currentHealth);
    }
    // Function for killing the player
    void Die()
    {   
        // Play Sound
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        // Play Effect
        Instantiate(explosion, transform.position, Quaternion.identity);
        gameObject.GetComponent<AudioListener>().enabled = false; // Disable the player audio listener before the player object is destroyed
        GameObject.Find("Main_Camera").GetComponent<AudioListener>().enabled = true; // active the backup listener on the camera.
        // Destroy the Player
        Destroy(gameObject);
        EventManager.GameOver();  // Signal the game over event
    }

    // When player collides with anything
    private void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<AudioManager>().Play("Collisions"); // Play collision sound effect
    }
}
