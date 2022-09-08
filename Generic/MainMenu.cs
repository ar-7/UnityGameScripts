using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This script handles the main menu buttons. These functions are called from the UI buttons
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads the next scene in line in the build order
    }

    public void QuitGame()
    {
        Application.Quit(); // closes the game back to windows
    }
}
