using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEdgeWarning : MonoBehaviour
{
    // This script handles the behaviour of the warning that shows when the player collides with the edge of the play area

    [SerializeField]GameObject warning; // Object that contains the warning text

    // Start is called before the first frame update
    void Start()
    {
        warning.SetActive(false); // Make sure the warning is off at game launch
    }

    // Function that triggers when something collides with the hitbox of the object the script is attached to
    private void OnCollisionEnter(Collision collision)
    {
        // If the object colliding is the player
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(ShowAndHideWarning(2.0f)); // Run the coroutine to flash the warning, arg is the delay in seconds
            StartCoroutine(ShowAndHideAlpha(2.0f)); // Run the coroutine to flash the transparency of the wall object, arg is the delay in seconds
        }        
    }
    // Flashes the warning
    IEnumerator ShowAndHideWarning(float delay)
    {
        warning.SetActive(true); // Sets warning active
        yield return new WaitForSeconds(delay); // waits for delay amount
        warning.SetActive(false); // sets warning inactive
    }

    // Calls the fade alpha coroutine, which changes the transparency of the wall object
    IEnumerator ShowAndHideAlpha(float delay)
    {
        StartCoroutine(FadeAlphaTo(0.5f, 1.0f)); // in this instance the args are 0.5 alpha and transition time of 1 second
        yield return new WaitForSeconds(delay); // waits for delay amount
        StartCoroutine(FadeAlphaTo(0.2f, 1.0f)); // in this instance the args are 0.2 alpha and transition time of 1 second
    }

    // Fades the alpha of an object
    IEnumerator FadeAlphaTo(float aValue, float aTime)
    {
        float alpha = gameObject.GetComponent<MeshRenderer>().material.color.a; // Gets the a or alpha of the mesh renderer of the current game object
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) // loop that runs the gradualy change of the alpha
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t)); // doing maths on the alpha
            gameObject.GetComponent<MeshRenderer>().material.color = newColor; // sets the changed colour to the game object
            yield return null;
        }
    }
}