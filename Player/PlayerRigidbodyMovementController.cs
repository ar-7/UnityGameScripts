using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Lines 31-65 Physics based movement calculations adapted from: https://www.youtube.com/watch?v=8-UBALp1xQc&t=1s
MIT License

Copyright (c) 2019 World of Zero

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

public class PlayerRigidbodyMovementController : MonoBehaviour
{
    // This script handles the physics based movement of the player

    public float velocity; // Player speed
    public float rotationSpeed; // Player turn rate
    public float horizontal; // input variable controlling left to right movement
    public float vertical; // input variable controlling up and down movement

    private new Rigidbody rigidbody; // variable for storing the rigidbody belonging to the object this script is attached to 

    // Use this for initialization
    void Start()
    {
        // Gets the rigidbody of the ship to use for movement
        // This is important because the movement is physics based as opposed to using transform for movement
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gets inputs from the default control scheme
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        // Calls the movement calculations
        UpdateMoveManualRotation();
    }
    private void UpdateMoveManualRotation()
    {
        engineEffects(); // Call engine effects
        // Uses rigid body physics to push the model around.
        this.rigidbody.AddForce(vertical * this.transform.forward * velocity * Time.deltaTime);
        var rotationAmount = rotationSpeed * Time.deltaTime * horizontal;
        this.rigidbody.AddTorque(0, rotationAmount, 0);
    }

    // Declaring Engine Effects
    public ParticleSystem engineLeftEffect;
    public ParticleSystem engineRightEffect;
    public ParticleSystem engineRevLeftEffect;
    public ParticleSystem engineRevRightEffect;

    void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("EngineSound"); // Play engine audio
    }
    void StopSound()
    {
        FindObjectOfType<AudioManager>().Stop("EngineSound"); // Play engine audio
    }


    // Big set of keypress calculations to trigger the correct engine effect.
    private void engineEffects()
    {
        // if fwd is pressed
        if (vertical > 0 && (engineLeftEffect.isPlaying == false || engineRightEffect.isPlaying == false))
        {
            PlaySound();
            engineLeftEffect.Play();
            engineRightEffect.Play();
            engineRevLeftEffect.Stop();
            engineRevRightEffect.Stop();
        }
        // if reverse is pressed
        if (vertical < 0 && (engineRevLeftEffect.isPlaying == false || engineRevRightEffect.isPlaying == false))
        {
            PlaySound();
            engineRevLeftEffect.Play();
            engineRevRightEffect.Play();
            engineLeftEffect.Stop();
            engineRightEffect.Stop();
        }
        // if right key is pressed with no fwd key
        else if ((vertical == 0 && horizontal > 0) && (engineLeftEffect.isPlaying == false))
        {
            PlaySound();
            engineLeftEffect.Play();
            engineRevLeftEffect.Stop();
            engineRevRightEffect.Stop();
        }
        // if right key is pressed but fwd is released
        else if ((vertical == 0 && horizontal > 0) && engineRightEffect.isPlaying == true)
        {
            engineRightEffect.Stop();
        }
        // if left key is pressed with no fwd key
        else if ((vertical == 0 && horizontal < 0) && (engineRightEffect.isPlaying == false))
        {
            PlaySound();
            engineRightEffect.Play();
            engineRevLeftEffect.Stop();
            engineRevRightEffect.Stop();
        }
        // if left key is pressed but fwd is released
        else if ((vertical == 0 && horizontal < 0) && engineLeftEffect.isPlaying == true)
        {
            engineLeftEffect.Stop();
        }
        // if no keys are pressed, stop all effects
        else if ((vertical == 0 && horizontal == 0) && ((engineLeftEffect.isPlaying == true || engineRightEffect.isPlaying == true) || (engineRevLeftEffect.isPlaying == true || engineRevRightEffect.isPlaying == true)))
        {
            StopSound();
            engineLeftEffect.Stop();
            engineRightEffect.Stop();
            engineRevLeftEffect.Stop();
            engineRevRightEffect.Stop();
        }
    }
}
