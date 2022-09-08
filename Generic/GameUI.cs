using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameUI : MonoBehaviour
{
    // This script handles varied game UI functions

    [SerializeField] GameObject gameUI; // Object for the UI itself
    [SerializeField] GameObject endGameMenu; // Object for the game over screen
    [SerializeField] GameObject gameSplashScreen; // Object for the splash screen
    [SerializeField] GameObject beginButton; // object for the begin button on the splash screen

    private void Start()
    {
        ShowSplashScreen(); // splash screen opens on game load
        EventManager.acceptShipInput = false; // disables input to make sure the ship doesn't start firing lasers when the player clicks begin. 
        Time.timeScale = 0; // stops time in the game, awaiting begin signal
    }

    // Event listeners
    private void OnEnable()
    {
        EventManager.onStartGame += ShowGameUI; // When the begin game event is called, run the function 
        EventManager.onGameOver += DelayMenuDisplay; // When the end game event is called, run the function 
    }

    private void OnDisable()
    {
        EventManager.onStartGame -= ShowGameUI;
        EventManager.onGameOver -= DelayMenuDisplay;
    }

    // Function that begins the game when button is clicked.
    public void Begin()
    {
        EventManager.StartGame(); // Calls the begin game event
    }

    // Function that makes sure the game over screen doesn't pop up the instant the player dies
    // They get to enjoy their explosion for a couple seconds
    void DelayMenuDisplay()
    {
        Invoke("ShowEndGameMenu", 2); // Calls the function after a couple seconds
    }

    // Enables splash screen and makes sure game UI is disabled
    void ShowSplashScreen()
    {
        gameSplashScreen.SetActive(true);
        gameUI.SetActive(false);
    }

    // Enables game over screen and makes sure game UI is disabled
    void ShowEndGameMenu()
    {
        endGameMenu.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0; // pauses time in the game
    }

    // Enables game UI and makes sure game UI is disabled
    void ShowGameUI()
    {
        gameSplashScreen.SetActive(false);
        gameUI.SetActive(true);
    }
}
