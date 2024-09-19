using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITPokemonSoundController : MonoBehaviour
{

    public AudioSource pkAudio;
    public ParaLensesButtonBehavior paraLenses;

    public GameObject GM;
    public Camera myDevice;
    public float maxVolume = 1f; // Maximum volume of the audio source.
    public float minVolume = 0.1f; // Minimum volume of the audio source.
    public float volumeChangeSpeed = 2f; // Speed of volume change.
    private float targetVolume;



    // Start is called before the first frame update
    void Start()
    {
        //pkAudio = GetComponent<AudioSource>();
 

        targetVolume = minVolume;
        pkAudio.volume = minVolume; // Start at the minimum volume.
    }

    void Update()
    {

        float turn = Vector3.Dot(GM.transform.forward, myDevice.transform.up);
        // Adjust the target volume based on turn input.
        if (turn != 0)
        {
            targetVolume = Mathf.Lerp(minVolume, maxVolume, Mathf.Abs(turn));
        }
        else
        {
            targetVolume = minVolume; // Lower the volume when no input.
        }

        // Smoothly change the volume to the target volume.
        pkAudio.volume = Mathf.Lerp(pkAudio.volume, targetVolume, Time.deltaTime * volumeChangeSpeed);
    }
    public void StopAudio()
    {
        pkAudio.Stop();
    }

    public void PlayPokemonAudio()
    {
        
            
            pkAudio.Play();
        }
    }
}
