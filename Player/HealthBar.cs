using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // This script handles the updating of the health bar in the UI
    // These functions are called from the player handler

    public Slider slider; // Object for the health bar which is a slider

    // Function that sets the player's max health
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Function that sets the player's current health
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
