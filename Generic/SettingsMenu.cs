using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    // This script handles the settings menu accessible via main menu
    // These functions are called from the buttons in the menu

    public AudioMixer audioMixer; // Var for storing the main audio mixer

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume); // modify the audio mixer level
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // set the graphics quality form the list
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; // set the fullscreen toggle
    }
}
