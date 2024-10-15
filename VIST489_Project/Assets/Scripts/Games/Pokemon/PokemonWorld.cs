using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class PokemonWorld : MonoBehaviour
{
    #region Singleton
    public static PokemonWorld instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of PokemonWorld!");
            return;
        }

        instance = this;
    }
    #endregion

    public GameObject entireMaze;
    private bool canGetKey = false;
    private bool unlockedDoor = false;
    private bool pickedUpKey = false;



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
    MazeBehavior mazeScript;

    // --------------------------------------

    private int count = 0; // Used to limit in update how many times the pop up is called if we tap on an object that has a pop up box appear after
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void SolvedMaze()
    {
        canGetKey = true;

    }

    public bool CanPickUpKey()
    {
        return canGetKey;
    }

    public void CannotPickUpKey()
    {
        GameSystem = GameSystemBehavior.instance;
        paraLenses = ParaLensesButtonBehavior.instance;
        mazeScript = MazeBehavior.instance;

        
        if (count == 0 || mazeScript.getFailed() == true)
        {
            count = 0;
            

            GameSystem.SetHaveMessage(true);
            GameSystem.SetMessageText(NeedToCompleteMazeText);
            // popUp = PopUpSystem.instance;

            // popUp.PopUp(NeedToCompleteMazeText);
            
            entireMaze.SetActive(true);
            mazeScript.MazeStarted();
            count += 1;
            //PopUpBoxButton.onClick.AddListener(SetCountForPopUpKey);
        }
        
    }
    

    public void SetUnlockedDoor(bool status)
    {
        unlockedDoor = status;
    }

    public bool GetUnlockedDoor()
    {
        return unlockedDoor;
    }

    public void SetPickedUpKey(bool status)
    {
        pickedUpKey = status;
    }

    // Check if we have the key or not
    public bool GetPickedUpKey()
    {
        return pickedUpKey;
    }
}
