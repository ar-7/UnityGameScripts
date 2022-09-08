using UnityEngine.Audio; // Unity engine audio components
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	// This script comprises the audio manager for the game
	// The game object it is attached to stores the catalogue of sounds the game uses

	public static AudioManager instance; // Defining persistent audio manager instance

	public AudioMixerGroup mixerGroup; // Var for storing the audio mixer group

	public Sound[] sounds; // Stores the list of sounds with properties defined by the sounds class

	void Awake()
	{
		// Ensures the persistent object remains if wanted
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		// Loops through the list of sound records defined in the editor
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>(); // Creates an audio source
			s.source.clip = s.clip; // gives the new source the file of the current record
			s.source.loop = s.loop; // defines whether it should loop

			s.source.outputAudioMixerGroup = mixerGroup; // Sets the mixer group
		}
	}

	// Function for playing a sound from the list
	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound); // Finds the sound wanted in the list

		// if no sound available, throw an error and don't crash, return instead.
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		// Sets the volume and pitch of the new sound
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)); 
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play(); // Plays the sound
	}

	// Function that will stop a looping sound
    public void Stop(string sound)
    {
		Sound s = Array.Find(sounds, item => item.name == sound); // Finds the sound wanted in the list

		// if no sound available, throw an error and don't crash, return instead.
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		// Sets the volume and pitch of the new sound
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Stop(); // Stop the sound
	}
}
