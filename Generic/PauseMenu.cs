using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // This script handles the behaviour for the pause menu and the game over screen

    public static bool gameIsPaused = false; // Flag to store whether the game is paused
    public GameObject pauseMenuUI; // Object reference for the pause UI
    public Text gameOverText; // Text that displays on the end game screen

    private void OnEnable()
    {
        EventManager.onGameOver += PopulateEndGameScreen;
    }

    private void OnDisable()
    {
        EventManager.onGameOver -= PopulateEndGameScreen;

    }
    // Update is called once per frame
    void Update()
    {
        // If the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the game is paused
            if (gameIsPaused)
            {
                Resume(); // close the pause menu
            }
            else // if it is not paused
            {
                Pause(); // pause the game and open pause menu
            }
        }    
    }
    
    // Fills the text in the game over screen with the current score and new or existing high score.
    // Pulls the score values from the score script.
    void PopulateEndGameScreen()
    {
        gameOverText.text = "Well done! You scored: " + Score.score + "\r\n High score is: " + Score.highScore + "!";
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hides the pause menu
        Time.timeScale = 1f; // Resumes time in the game world
        gameIsPaused = false; // Set pause flag to false
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Shows the pause menu
        Time.timeScale = 0f; // Freezes time in the game world
        gameIsPaused = true; // Set pause flag to true
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Loads the previous scene in the heirarchy
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads current scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Closes the application
    }
}
