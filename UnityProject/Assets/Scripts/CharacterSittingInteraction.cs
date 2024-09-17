using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterSittingInteraction : MonoBehaviour
{
    public string popUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PopUpSystem pop = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PopUpSystem>();
        pop.PopUp(popUp);
    }
}
