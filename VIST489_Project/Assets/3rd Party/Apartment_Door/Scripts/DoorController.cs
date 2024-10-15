using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DoorController : MonoBehaviour
{
    #region Singleton
    public static DoorController instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of DoorController!");
            return;
        }

        instance = this;
    }
    #endregion
    public bool keyNeeded = false;              //Is key needed for the door
    public bool gotKey;                  //Has the player acquired key
    public GameObject keyGameObject;            //If player has Key,  assign it here
    public GameObject txtToDisplay;             //Display the information about how to close/open the door

    private bool playerInZone;                  //Check if the player is in the zone
    private bool doorOpened;                    //Check if door is currently opened or not

    private Animation doorAnim;
    private BoxCollider doorCollider;           //To enable the player to go through the door if door is opened else block him


    // Instance references to scripts
    PopUpSystem popUp;
    PokemonWorld pokeWorld;
    GameSystemBehavior gameSystem;
    ParaLensesButtonBehavior paraLensesScript;
    public string FindKeyTextForDoor = "He's right the door is locked, we need to find the key. Look inside your book you should have a map of the building. Use that to guide you";
    public string UseKeyTextForDoor = "Great we have the key now. Try tapping the key in your inventory to unlock or lock the door.";

    enum DoorState
    {
        Closed,
        Opened,
        Jammed
    }

    DoorState doorState = new DoorState();      //To check the current state of the door

    /// <summary>
    /// Initial State of every variables
    /// </summary>
    private void Start()
    {
        gotKey = false;
        doorOpened = false;                     //Is the door currently opened
        playerInZone = false;                   //Player not in zone
        doorState = DoorState.Closed;           //Starting state is door closed

        txtToDisplay.SetActive(false);

        doorAnim = transform.parent.gameObject.GetComponent<Animation>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider>();

        //If Key is needed and the KeyGameObject is not assigned, stop playing and throw error
        if (keyNeeded && keyGameObject == null)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            Debug.LogError("Assign Key GameObject");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        txtToDisplay.SetActive(true);
        playerInZone = true;

        popUp = PopUpSystem.instance;
        gameSystem = GameSystemBehavior.instance;
        paraLensesScript = ParaLensesButtonBehavior.instance;
        pokeWorld = PokemonWorld.instance;

        if(pokeWorld.GetPickedUpKey() == false)
        {
            if (gameSystem.GetEnteredPokemonWorld() == true && paraLensesScript.getIsParaLensesOn() && pokeWorld.GetUnlockedDoor() == false)
            {
                gameSystem.SetHaveMessage(true);
                gameSystem.SetMessageText(FindKeyTextForDoor);
                
                    
            }
        }
        else if(pokeWorld.GetPickedUpKey() == true)
        {
            if (gameSystem.GetEnteredPokemonWorld() == true && paraLensesScript.getIsParaLensesOn() && pokeWorld.GetUnlockedDoor() == false)
            {
                gameSystem.SetHaveMessage(true);
                gameSystem.SetMessageText(FindKeyTextForDoor);
                    
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInZone = false;
        txtToDisplay.SetActive(false);
    }

    private void Update()
    {
        //To Check if the player is in the zone
        if (playerInZone)
        {
            if (doorState == DoorState.Opened)
            {
                txtToDisplay.GetComponent<TextMeshProUGUI>().text = "Press 'E' to Close";
                doorCollider.enabled = false;
            }
            else if (doorState == DoorState.Closed || gotKey)
            {
                txtToDisplay.GetComponent<TextMeshProUGUI>().text = "Press 'E' to Open";
                doorCollider.enabled = true;
            }
            else if (doorState == DoorState.Jammed)
            {
                txtToDisplay.GetComponent<TextMeshProUGUI>().text = "Needs Key";
                doorCollider.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && playerInZone)
        {
            doorOpened = !doorOpened;           //The toggle function of door to open/close

            if (doorState == DoorState.Closed && !doorAnim.isPlaying)
            {
                if (!keyNeeded)
                {
                    doorAnim.Play("Door_Open");
                    doorState = DoorState.Opened;
                }
                else if (keyNeeded && !gotKey)
                {
                    if (doorAnim.GetClip("Door_Jam") != null)
                        doorAnim.Play("Door_Jam");
                    doorState = DoorState.Jammed;
                }
            }

            if (doorState == DoorState.Closed && gotKey && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            }

            if (doorState == DoorState.Opened && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Close");
                doorState = DoorState.Closed;
            }

            if (doorState == DoorState.Jammed && !gotKey)
            {
                if (doorAnim.GetClip("Door_Jam") != null)
                    doorAnim.Play("Door_Jam");
                doorState = DoorState.Jammed;
            }
            else if (doorState == DoorState.Jammed && gotKey && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            }
        }

        
    }

    public void ToggleDoor()
    {
        Debug.Log("Here " + gotKey + " " + keyNeeded + " " + playerInZone);
        if (playerInZone)
        {
            doorOpened = !doorOpened;           //The toggle function of door to open/close

            if (doorState == DoorState.Closed && !doorAnim.isPlaying)
            {
                if (!keyNeeded)
                {
                    doorAnim.Play("Door_Open");
                    doorState = DoorState.Opened;
                }
                else if (keyNeeded && !gotKey)
                {
                    if (doorAnim.GetClip("Door_Jam") != null)
                        doorAnim.Play("Door_Jam");
                    doorState = DoorState.Jammed;
                }
            }

            if (doorState == DoorState.Closed && gotKey && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            }

            if (doorState == DoorState.Opened && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Close");
                doorState = DoorState.Closed;
            }

            if (doorState == DoorState.Jammed && !gotKey)
            {
                if (doorAnim.GetClip("Door_Jam") != null)
                    doorAnim.Play("Door_Jam");
                doorState = DoorState.Jammed;
            }
            else if (doorState == DoorState.Jammed && gotKey && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            }
        }
    }
}
