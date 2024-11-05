using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphGrower : MonoBehaviour
{

    public float growthRate = 0.1f; // Adjust this value to control how fast the object grows
    private bool isGrowing = false;
    public bool hasBeenSized = false;
    public float timer = 0;
    public float maxTime = 10.0f;
    public bool hasPlayedSound = false;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(.1f, .1f, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasBeenSized)
        {
            // Check for touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Start growing on touch hold
                //if (touch.phase == TouchPhase.Stationary)
                
                isGrowing = true;
                
                //else
                //{
                 //   isGrowing = false;
                 //   hasBeenSized = true;
                //}
            }
            else
            {
                isGrowing = false;
                //hasBeenSized = true;
            }

            // Grow the object if the user is holding their finger down
            if (isGrowing)
            {
                this.transform.localScale += new Vector3(1f, 1f, 1f) * growthRate * Time.deltaTime;

                if(this.transform.localScale.magnitude >= 1.0f)
                {
                    if (!hasPlayedSound)
                    {
                        //play sound
                        source.Play();
                        hasPlayedSound = true;
                        hasBeenSized = true;
                    }
                }
            }
        }
    }
}
