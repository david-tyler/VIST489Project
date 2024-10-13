using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectApproach : MonoBehaviour
{
    PopUpSystem popUp;

    public string YouNeedToFindTheDoorText = "This door seems to be locked in the pokemon world. The key should be somewhere on this floor! Perhaps the book has more clues?";
    void OnCollisionEnter(Collision collision)
    {
        popUp = PopUpSystem.instance;
        
        popUp.PopUp(YouNeedToFindTheDoorText);
    }
    
}
