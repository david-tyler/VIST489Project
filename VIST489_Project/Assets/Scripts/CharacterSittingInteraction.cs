using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterSittingInteraction : MonoBehaviour
{
    public string popUp;
    private PopUpSystem popUpSystem; // Cache the reference
    
    // Start is called before the first frame update
    void Start()
    {
        // Cache the PopUpSystem reference once at start
        popUpSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PopUpSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Use cached reference instead of finding it every frame
        if (popUpSystem != null)
        {
            popUpSystem.PopUp(popUp);
        }
    }
}
