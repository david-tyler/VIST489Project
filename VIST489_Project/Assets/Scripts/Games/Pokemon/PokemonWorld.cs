using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class PokemonWorld : MonoBehaviour
{
    public GameObject key;
    public GameObject entireMaze;
    private bool canGetKey = false;

    // ******* PopUp Boxes
    // --------------------------------------
    public GameObject PopUpBox;
    public Animator PopUpBoxAnimator;
    public TMPro.TextMeshProUGUI PopUpBoxText;
    public Button PopUpBoxButton;
    public string NeedToCompleteMazeText = "It seems we can't get the key, but look there! On the ground to your left, there seems to be a maze. Maybe you need to complete it to obtain the key.";
    public string YouNeedToFindTheKeyText = "This door seems to be locked in the pokemon world. The key should be somewhere on this floor! Perhaps the book has more clues?";

    GameSystemBehavior GameSystem;
    ParaLensesButtonBehavior paraLenses;
    PopUpSystem popUp;
    // --------------------------------------

    private int count = 0; // Used to limit in update how many times the pop up is called if we tap on an object that has a pop up box appear after
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void SolvedMaze()
    {
        key.SetActive(true);
        canGetKey = true;

    }

    public bool CanPickUpKey()
    {
        return canGetKey;
    }

    public void CannotPickUpKey()
    {
        if (count == 1)
        {
            PopUpBoxButton.onClick.RemoveListener(SetCountForPopUpKey);
            return;
        }
        else if (count == 0)
        {
            GameSystem = GameSystemBehavior.instance;
            paraLenses = ParaLensesButtonBehavior.instance;

            GameSystem.SetHaveMessage(true);
            GameSystem.SetMessageText(NeedToCompleteMazeText);
            // popUp = PopUpSystem.instance;

            // popUp.PopUp(NeedToCompleteMazeText);
            
            entireMaze.SetActive(true);
            count += 1;
            PopUpBoxButton.onClick.AddListener(SetCountForPopUpKey);
        }
        
    }
    

    // so you dont spam the popup if you keep clicking buttons on top of the key
    public void  SetCountForPopUpKey()
    {
        count = 0;
    }

}
