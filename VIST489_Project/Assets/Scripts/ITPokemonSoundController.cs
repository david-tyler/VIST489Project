using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITPokemonSoundController : MonoBehaviour
{

    private AudioSource pkAudio;

   

    // Start is called before the first frame update
    void Start()
    {
        pkAudio = GetComponent<AudioSource>();
        
    }


    public void PlayPokemonAudio()
    {
        
            
        pkAudio.Play();
    }
}
