using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // This script handles the behaviour of the score mechanic

    [SerializeField] Text scoreText; // UI display for current score
    [SerializeField] Text highScoreText; // UI diplay for high score from player prefs

    public static int score; // var to store current score
    public static int highScore; // var to store high score

    // Event listener
    private void OnEnable()
    {
        // Activate listeners for events
        EventManager.onStartGame += LoadHighScore; 
        EventManager.onStartGame += ResetScore;
        EventManager.onScorePoints += AddScore;
        EventManager.onGameOver += CheckNewHighScore;
    }

    private void OnDisable()
    {
        // Disable listeners for event
        EventManager.onStartGame -= LoadHighScore;
        EventManager.onStartGame -= ResetScore;        
        EventManager.onScorePoints -= AddScore;
        EventManager.onGameOver -= CheckNewHighScore;
    }
    
    // Function that resets the current score
    void ResetScore()
    {
        score = 0;
        DisplayScore();
    }

    // Function that adds points gained to the score. Takes in an imount of poitn as an arg
    void AddScore(int amount)
    {
        score += amount;
        DisplayScore();
    }

    // Function that updates the current score UI display
    void DisplayScore()
    {
        scoreText.text = score.ToString();
    }

    // Function that loads the high score from player prefs, then calls the function that updates the UI display
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        DisplayHighScore();
    }

    // Function to update the UI display for the high score
    void DisplayHighScore()
    {
        highScoreText.text = highScore.ToString();
    }

    // Compares the existing high score with the current score and updates if it is higher
    void CheckNewHighScore()
    {
        if (score>highScore)
        {
            PlayerPrefs.SetInt("highScore", score); // Save value to player prefs
            LoadHighScore(); // update UI display
        }
    }
}
