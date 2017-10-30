﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    //Declare Global Variables
    
    //End Result - The volume of the microphone's input, but normalized
    public static float normalizedMicrophoneInput;
    //Microphone reference
    private string inputDevice;
    //Microphone Input
    private AudioClip audioClip;
    //Sample Rate
    int sampleWindow = 128;
    //Test Variable
    public float soundLevel;
    //Test Light
    public Light light;
    //Test Intensity
    private float intensity = 0;

	// Use this for initialization
	void Start ()
    {
		if(inputDevice == null)
        {
            inputDevice = Microphone.devices[0];
            audioClip = Microphone.Start(inputDevice, true, 999, 44100);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        normalizedMicrophoneInput = MaxVolume();
        soundLevel = normalizedMicrophoneInput;

        if (intensity < soundLevel)
        {
            intensity = soundLevel;
        }
        else
        {
            intensity -= 0.01f;
        }

        light.intensity = intensity;
	}

    //Get a Normalised Volume Peak from the Sampled Audio Clip
    float MaxVolume()
    {
        //Float to return
        float maxVolume = 0;
        //Array of Samples from the Audio Input
        float[] clipSampleData = new float[sampleWindow];
        //Position of the Sample
        int micPosition = Microphone.GetPosition(inputDevice) - (sampleWindow + 1);

        //Error Checking
        if (micPosition < 0)
        {
            return 0;
        }

        //Populating the Array from the audio input
        audioClip.GetData(clipSampleData, micPosition);
        
        //For each sample
        for (int i = 0; i < sampleWindow; i++)
        {
            //Get the peak volume by Squaring the Data Sample (Normalizing)
            float wavePeak = clipSampleData[i] * clipSampleData[i];
            
            //If the volume isn't about a certain threshold
            if(wavePeak < 0.01)
            {
                wavePeak = 0;
            }

            //Else if the volume is above the previously sampled volume, save it.
            if (maxVolume < wavePeak)
            {
                maxVolume = wavePeak;
            }
        }
        
        //return the float
        return maxVolume;
    }
}
