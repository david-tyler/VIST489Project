using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITPokemonSoundController : MonoBehaviour
{

    private AudioSource pkAudio;
    private ParaLensesButtonBehavior paraLenses;

    public GameObject GM;
    public Camera myDevice;

    // Start is called before the first frame update
    void Start()
    {
        pkAudio = GetComponent<AudioSource>();
        paraLenses = GM.GetComponent<ParaLensesButtonBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayPokemonAudio()
    {
        
        if (paraLenses.getIsParaLensesOn())
        {
            
            pkAudio.Play();
        }


        
    }
}
