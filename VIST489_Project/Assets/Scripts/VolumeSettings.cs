using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Transform image;

    const string Mixer_SFX = "SFXVolume";
    private float prevRotation_Y;
    private float currRotation_Y;
    private float volume;

    private float minVolume = -60.0f;
    private float maxVolume = 7.5f;

    [SerializeField]
    private float AngleIncrement = 20.0f;

    [SerializeField]
    private float VolumeIncrement = 1.0f;
    
    // Optimization: Cache the last rotation to avoid unnecessary calculations
    private float lastCheckedRotation;
    private const float rotationChangeThreshold = 0.1f; // Only check if rotation changed significantly
    
    // Start is called before the first frame update
    void Start()
    {
        prevRotation_Y = image.eulerAngles.y;
        lastCheckedRotation = prevRotation_Y;
        mixer.GetFloat(Mixer_SFX, out volume);
    }

    // Update is called once per frame
    void Update()
    {
        currRotation_Y = image.eulerAngles.y;

        // Only proceed if rotation has changed significantly
        if (Mathf.Abs(currRotation_Y - lastCheckedRotation) < rotationChangeThreshold)
            return;
            
        lastCheckedRotation = currRotation_Y;
        
        if (currRotation_Y >= prevRotation_Y + AngleIncrement)
        {
            Debug.Log(volume);
            volume = Mathf.Clamp(volume + VolumeIncrement, volume, maxVolume);
            SetAudioVolume(volume);
            prevRotation_Y = currRotation_Y;
        }
        else if (currRotation_Y <= prevRotation_Y - AngleIncrement)
        {

            Debug.Log(volume);
            volume = Mathf.Clamp(volume - VolumeIncrement, minVolume, maxVolume);
            SetAudioVolume(volume);
            prevRotation_Y = currRotation_Y;
        }

    }

    
    private void SetAudioVolume(float value)
    {
        mixer.SetFloat(Mixer_SFX, value) ;
    }
}
