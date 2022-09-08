using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // This script manages the behaviour of the timer mechanic

    public Text timerText; // the text object which displays the timer to the player.
    public float timeLeft = 120; // float to store the time logging
    bool logTime; // boolean to signal whether time is being logged
    public GameObject timeBonusForKill; // The pop-up that lets the player know they received extra time
    public GameObject timeBonusForPickup; // The pop-up that lets the player know they received extra time


    void OnEnable()
    {
        // Engage event listeners
        EventManager.onStartGame += StartTimer;
        EventManager.onGameOver += StopTimer;
        EventManager.onEnemyDeath += TimeBonueForKill;
        EventManager.onSpawnNewPickup += TimeBonusForPickup;
    }

    void OnDisable()
    {
        // Disengage event listeners
        EventManager.onStartGame -= StartTimer;
        EventManager.onGameOver -= StopTimer;
        EventManager.onEnemyDeath -= TimeBonueForKill;
        EventManager.onSpawnNewPickup -= TimeBonusForPickup;
    }

    // This happens every frame
    void Update()
    {
        // If the timer is supposed to be running, tick down
        if (logTime)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        // If the timer is empty
        if (timeLeft < 0)
        {
            StopTimer(); // Stop the timer
            timerText.text = "Time's up!"; // Change text to a message
            EventManager.GameOver();  // Signal the game over event
        }
    }

    void StartTimer()
    {
        logTime = true; // Signal the timer to start
    }

    void StopTimer()
    {
        logTime = false; // Signal the timer to stop
    }

    // Updates the timer display
    void UpdateTimerDisplay()
    {
        int minutes;
        float seconds;

        minutes = Mathf.FloorToInt(timeLeft/60); // Do some maths to get minutes passed
        seconds = timeLeft % 60; // remainder of divide by 60

        timerText.text = string.Format("{0}:{1:00.00}", minutes, seconds); // Apply the timer text using the custom format
    }

    // Give bonus on event
    void TimeBonueForKill()
    {
        // Add time to timer
        timeLeft += 5;
        StartCoroutine(HideAndShowKillTime(1.0f)); // flash the amount in the UI
    }

    IEnumerator HideAndShowKillTime(float delay)
    {
        timeBonusForKill.SetActive(true);
        yield return new WaitForSeconds(delay);
        timeBonusForKill.SetActive(false);
    }

    // Give bonus on event
    void TimeBonusForPickup()
    {
        // Add time to timer
        timeLeft += 20;
        StartCoroutine(HideAndShowPickupTime(1.0f)); // flash the amount in the UI
    }

    IEnumerator HideAndShowPickupTime(float delay)
    {
        timeBonusForPickup.SetActive(true);
        yield return new WaitForSeconds(delay);
        timeBonusForPickup.SetActive(false);
    }
}
