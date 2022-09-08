using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // This script contains the event triggers that other scripts listen for in order to carry out their behaviours at the correct time.
    // Other scripts call the events listed here when appropriate to trigger them.

    public static bool acceptShipInput; // Switch for disabling input to the player ship, used to keep the ship from firing with mouse clicks in the splash screen.

    public delegate void GameDelegate(); // Delegate that listens for events that need no args 

    // Triggers for each individual event
    public static GameDelegate onStartGame; 
    public static GameDelegate onGameOver;
    public static GameDelegate onEnemyDeath;
    public static GameDelegate onSpawnNewPickup;
    public static GameDelegate onPickedUp;

    public delegate void ScorePointsDelegate(int amount); // Delegate listening for the score even, which needs an arg
    public static ScorePointsDelegate onScorePoints; // Trigger for the score event

    // Trigger for game start event
    // Called by the begin game button
    public static void StartGame()
    {
        if (onStartGame != null)
        {
            onStartGame();
            Time.timeScale = 1; // Unpauses time in the game
            acceptShipInput = true; // Enables control of the player ship
        }
    }

    // Trigger for game over event
    public static void GameOver()
    {
        if (onGameOver != null)
        {
            onGameOver();
            acceptShipInput = false; // Makes sure the player ship can't be controlled if the time runs out and it is still alive. 
        }
    }

    // Trigger for enemy death event
    public static void EnemyDeath()
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath();
        }
    }

    // Trigger for new pickups event
    public static void SpawnNewPickup()
    {
        if (onSpawnNewPickup != null)
        {
            onSpawnNewPickup();
        }
    }

    // Trigger for point scoring event
    public static void ScorePoints(int score)
    {
        if (onScorePoints != null)
        {
            onScorePoints(score);
        }
    }
}
