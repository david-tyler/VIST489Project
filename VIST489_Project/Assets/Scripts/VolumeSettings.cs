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
    
    // Start is called before the first frame update
    void Start()
    {
        prevRotation_Y = image.eulerAngles.y;
        mixer.GetFloat(Mixer_SFX, out volume);
    }

    // Update is called once per frame
    void Update()
    {
        currRotation_Y = image.eulerAngles.y;

        
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
