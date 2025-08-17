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
    
    // Cache previous values to avoid unnecessary calculations
    private Vector3 previousGMForward;
    private Vector3 previousCameraUp;
    private float previousTurn;



    // Start is called before the first frame update
    void Start()
    {
        //pkAudio = GetComponent<AudioSource>();
 

        targetVolume = minVolume;
        pkAudio.volume = minVolume; // Start at the minimum volume.
        
        // Initialize previous values
        previousGMForward = GM.transform.forward;
        previousCameraUp = myDevice.transform.up;
        previousTurn = 0f;
    }

    void Update()
    {
        // Only calculate if transform has changed
        Vector3 currentGMForward = GM.transform.forward;
        Vector3 currentCameraUp = myDevice.transform.up;
        
        if (currentGMForward != previousGMForward || currentCameraUp != previousCameraUp)
        {
            float turn = Vector3.Dot(currentGMForward, currentCameraUp);
            
            // Only update if turn value actually changed
            if (Mathf.Abs(turn - previousTurn) > 0.001f)
            {
                // Adjust the target volume based on turn input.
                if (turn != 0)
                {
                    targetVolume = Mathf.Lerp(minVolume, maxVolume, Mathf.Abs(turn));
                }
                else
                {
                    targetVolume = minVolume; // Lower the volume when no input.
                }
                
                previousTurn = turn;
            }
            
            previousGMForward = currentGMForward;
            previousCameraUp = currentCameraUp;
        }

        // Only update volume if target is different from current
        if (Mathf.Abs(pkAudio.volume - targetVolume) > 0.001f)
        {
            // Smoothly change the volume to the target volume.
            pkAudio.volume = Mathf.Lerp(pkAudio.volume, targetVolume, Time.deltaTime * volumeChangeSpeed);
        }
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
