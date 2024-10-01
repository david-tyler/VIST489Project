using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonWorld : MonoBehaviour
{
    public GameObject key;
    public GameObject entireMaze;
    private bool canGetKey = false;

    // ******* PopUp Boxes
    // --------------------------------------
    public GameObject NeedToCompleteMazePopUp;
    public Animator NeedToCompleteMazePopUpAnimator;
    public TMPro.TextMeshProUGUI text_NeedToCompleteMazePopUp;

    PopUpSystem popUp;
    // --------------------------------------
    private int count = 0;
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
            return;
        }
        else if (count == 0)
        {
            popUp = PopUpSystem.instance;
            popUp.popUpBox = NeedToCompleteMazePopUp;
            popUp.popUpText = text_NeedToCompleteMazePopUp;
            popUp.animator = NeedToCompleteMazePopUpAnimator;
            popUp.PopUp(text_NeedToCompleteMazePopUp.text);
            entireMaze.SetActive(true);
            count += 1;
        }
        
    }

    // so you dont spam the popup if you keep clicking buttons on top of the key
    public void  SetCountForPopUpKey()
    {
        count = 0;
    }

}
