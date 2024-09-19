using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNarration : MonoBehaviour
{
    public AudioSource source;
    private bool playing = false;
    public float timeToWait = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToWait -= Time.deltaTime;

        if (timeToWait <= 0.0f)
        {
            if(!playing)
            {
                source.Play();
            }
            playing = true;
        }
    }
}
