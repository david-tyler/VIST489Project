using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSystemBehavior : MonoBehaviour
{

    #region Singleton
    public static GameSystemBehavior instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of GameSystemBehavior!");
            return;
        }

        instance = this;
    }
    #endregion

    // Keep track of what interactable we have focused
    private Interactable focus;
  
    public GameObject ParaNormalLensesGameObject;
    public Button ParaNormalLensesButton;
    public Camera mainCamera;

    // Narration Audio Sources
    // --------------------------------------
    public AudioSource introAudio;
    public AudioSource FindAGatewayAudio;
    public AudioSource TapOnIt;
    public AudioSource ThatBook;
    // --------------------------------------

    // ******* PopUp Boxes
    // --------------------------------------
    PopUpSystem popUp;
    public GameObject PopUpBox; // Our dialogue box
    public Animator PopUpBoxAnimator;
    public TMPro.TextMeshProUGUI PopUpBoxText;
    // --------------------------------------


    // ******* Image Targets that need to be scanned to enter respective World
    // --------------------------------------
    public GameObject ImageTargetEnterPokemonWorld;
    public GameObject ImageTargetEnterMarioWorld;
    public GameObject ImageTargetEnterZeldaWorld;
    // --------------------------------------

    

    // ******* Specific objects related to the Pokemon World
    // --------------------------------------
    // *** GameObjects & Buttons
    public GameObject PokemonWorld;
    public GameObject PokemonWorldButtonGameObject;
    public GameObject LeavePokemonWorldButtonGameObject;
    public GameObject InventoryButtonGameObject; // the title button for the inventory to toggle it on/off.
    public GameObject MessageNotification;

    public Button PokemonWorldButton;
    public Button LeavePokemonWorldButton;
    public GameObject MessageButton;
    // --------------------------------------


    // ******* Strings
    // --------------------------------------
    private string display_EnterPokemonWorld = "Enter Pokemon World";
    private string display_LeavePokemonWorld = "Leave Pokemon World";
    private string CurrentWorld = "";
    private string MessageText = "";
    // --------------------------------------


    // ******* Booleans for different conditions
    // --------------------------------------
    private bool gameStarted = false;
    // boolean to track if we have entered a world or not at least once for the introduction audio behavior
    private bool EnteredPokemonWorld = false;

    private bool AreWeInAWorld = false;
    private bool haveMessage = false;
    // --------------------------------------




    // ******* Text
    // --------------------------------------
    public TMPro.TextMeshProUGUI text_LeavePokemonWorldButtonGameObject;
    public TMPro.TextMeshProUGUI text_PokemonWorldButtonGameObject;
    
    // --------------------------------------

    /* Variable for functionality to enable or disable certain worlds only if previous worlds have been completed
     *  if PreviosWorldsCompleted == 0, we can only enter the Pokemon world
     *  if PreviosWorldsCompleted == 1, we can only enter the Pokemon world and the Mario World
     *  if PreviosWorldsCompleted == 2, we can only enter the Pokemon world and the Mario World and the Zelda World
    */
    private int PreviousWorldsCompleted = 0;

    // All of the game objects we want to not be active once we start the game so like puzzles etc.
    public List<GameObject> gameObjectsNotActive = new List<GameObject>();

    ParaLensesButtonBehavior paraLensesScript;
    void Start()
    {
        paraLensesScript = ParaLensesButtonBehavior.instance;

        // if our lenses are on turn on the image targets for certain worlds so we can start tracking
        paraLensesScript.ActiveGameObjects.Add(ImageTargetEnterPokemonWorld);
        paraLensesScript.ActiveGameObjects.Add(ImageTargetEnterMarioWorld);
        paraLensesScript.ActiveGameObjects.Add(ImageTargetEnterZeldaWorld);
        paraLensesScript.ActiveGameObjects.Add(ImageTargetEnterZeldaWorld);
        paraLensesScript.ActiveGameObjects.Add(InventoryButtonGameObject);

        foreach (GameObject item in gameObjectsNotActive)
        {
            item.SetActive(false);
        }
        
    }

    void Update()
    {
        
        
        // enabling and disabling interacting with the button while the audio is playing
        // Using return because i dont want the game to recognize player touch input on the screen before they have instructions
        // and accidentaly touch something they're not supposed to.

        if (introAudio.isPlaying == true)
        {
            ParaNormalLensesButton.interactable = false;
            return;
        } // Audios for the introduction setting up the game

        else if (introAudio.isPlaying == false)
        {
            ParaNormalLensesButton.interactable = true;

            if (FindAGatewayAudio.isPlaying == true)
            {
                ParaNormalLensesButton.interactable = false;
                return;
            }
            else if (FindAGatewayAudio.isPlaying == false)
            {
                ParaNormalLensesButton.interactable = true;
            }
            
        }

        if (TapOnIt.isPlaying == true)
        {
            PokemonWorldButton.interactable = false;
            return;
        }
        else if (TapOnIt.isPlaying == false)
        {
            PokemonWorldButton.interactable = true;
            
        }

        // *******Touch Interactions
        // --------------------------------------
        // Check if there is at least one touch.
        if (Input.touchCount > 0)
        {
            // Debug.Log("TOUCHING SCREEN");
            Touch touch = Input.GetTouch(0);

            // Check if the touch is just beginning.
            if (touch.phase == TouchPhase.Began)
            {
                // Debug.Log("TOUCHING Begin");

                // Create a ray from the screen point where the touch occurred.
                Ray ray = mainCamera.ScreenPointToRay(touch.position);

                // Variable to store the hit information.
                RaycastHit hit;

                // Perform the raycast.
                if (Physics.Raycast(ray, out hit))
                {
                    // Call a function on the object hit.
                    // Debug.Log("Tapped on " + hit.collider.gameObject.name);
                    hit.collider.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);

                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        ItemPickup currentItem = hit.collider.GetComponent<ItemPickup>();

                        
                        // uncomment once you figure out how you enter the pokemon world
                        if (EnteredPokemonWorld == true)
                        {
                            PokemonWorld pokeWorld = gameObject.GetComponent<PokemonWorld>();
                            string name = currentItem.item.name;

                            switch (name)
                            {
                                case "Key":
                                    if (pokeWorld.CanPickUpKey() == true)
                                    {
                                        SetFocus(interactable);
                                    }
                                    else if (pokeWorld.CanPickUpKey() == false)
                                    {

                                        pokeWorld.CannotPickUpKey();

                                    }
                                    break;
                                case "Red Glyph Pokeball":
                                    SetFocus(interactable);
                                    break;
                                case "Yellow Glyph Pokeball":
                                    SetFocus(interactable);
                                    break;
                                case "Green Glyph Pokeball":
                                    SetFocus(interactable);
                                    break;
                                case "Blue Glyph Pokeball":
                                    SetFocus(interactable);
                                    break;
                            }
                        }

                        

                    }
                    else
                    {
                        RemoveFocus();
                    }
                }
                else
                {
                    RemoveFocus();
                }

                if (Physics.Raycast(ray, out hit))
                {
                    // Try to find the IRaycastHitHandler interface on the hit object
                    IRaycastHitHandler hitHandler = hit.collider.GetComponent<IRaycastHitHandler>();

                    // If the object has the interface, fire the event
                    if (hitHandler != null)
                    {
                        hitHandler.OnRaycastHit();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) // 0 is for left-click
        {

            // Create a ray from the screen point where the touch occurred.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Variable to store the hit information.
            RaycastHit hit;

            // Perform the raycast.
            if (Physics.Raycast(ray, out hit))
            {

                // Check if the object hit has the tag "Interactable" (optional).
                //if (hit.collider != null && hit.collider.CompareTag("Interactable"))
                //{
                    // Call a function on the object hit.
                    // Debug.Log("Tapped on " + hit.collider.gameObject.name);
                    hit.collider.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);
                //

                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    ItemPickup currentItem = hit.collider.GetComponent<ItemPickup>();
                    if (EnteredPokemonWorld == true)
                    {
                        PokemonWorld pokeWorld = gameObject.GetComponent<PokemonWorld>();

                        string name = currentItem.item.name;
                            
                        switch (name)
                        {
                            case "Key":
                                if (pokeWorld.CanPickUpKey() == true)
                                {
                                    SetFocus(interactable);
                                }
                                else if (pokeWorld.CanPickUpKey() == false)
                                {

                                    pokeWorld.CannotPickUpKey();

                                }
                                break;
                            case "Red Glyph Pokeball":
                                SetFocus(interactable);
                                break;
                            case "Yellow Glyph Pokeball":
                                SetFocus(interactable);
                                break;
                            case "Green Glyph Pokeball":
                                SetFocus(interactable);
                                break;
                            case "Blue Glyph Pokeball":
                                SetFocus(interactable);
                                break;
                        }
                    }
                    

                }
                else
                {
                    RemoveFocus();
                }
            }
            else
            {
                RemoveFocus();
            }

        }
        // --------------------------------------

        if (haveMessage == true)
        {
            MessageNotification.SetActive(true);
        }
        else
        {
            MessageNotification.SetActive(false);
        }



        

    }

    // focus means that we clicked on an item and we are focused on that item to pick it up.
    // idea better works with 3d games when you click an item and your character moves to it
    void SetFocus (Interactable newFocus)
    {
        if (newFocus != focus)
        {
            // if we have a previous focus once we get a new one
            if (focus != null)
            {
                focus.OnDefocused();
            }
            
            focus = newFocus;
            
        }

        newFocus.OnFocused(mainCamera.transform);
    }

    void RemoveFocus ()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }

        focus = null;
    }

    public void PlayAudios()
    {
        paraLensesScript = ParaLensesButtonBehavior.instance;
        bool isParaLensesOn = paraLensesScript.getIsParaLensesOn();
        // With this functionality the Find A Gateway voice line only plays if we enable the paranormal lenses
        if (isParaLensesOn == true)
        {
            if (AreWeInAWorld == false)
            {
                if (PreviousWorldsCompleted == 0)
                {
                    if (
                        TapOnIt.isPlaying == false
                        && ThatBook.isPlaying == false
                        )
                    {
                        FindAGatewayAudio.Play();
                    }


                    if (
                        FindAGatewayAudio.isPlaying == false
                        && ThatBook.isPlaying == false
                        )
                    {
                        TapOnIt.Play();
                    }

                    if (
                        FindAGatewayAudio.isPlaying == false
                        && TapOnIt.isPlaying == false
                        )
                    {
                        ThatBook.Play();
                    }
                }
                
            }
            
        }

        
        
    }

   


    public void BeginGame()
    {
        // set this to true cause once we hit that intial ok button we have started the game
        gameStarted = true;
        
        if (gameStarted == true)
        {
            // Fuctionality to play the intro Audio so now it doesn't constantly play
            StartCoroutine(SetUpIntroduction());
            

        }

        

        
    }
    IEnumerator SetUpIntroduction()
    {
        // turn on the paranormal lenses
        
        
        yield return new WaitForSeconds(2);
        introAudio.Play();
        yield return new WaitForSeconds(introAudio.clip.length);
        ParaNormalLensesGameObject.SetActive(true);

        // Follow this as a guide to modify pop up boxes where you want
        popUp = PopUpSystem.instance;
        popUp.popUpBox = PopUpBox;
        popUp.popUpText = PopUpBoxText;
        popUp.animator = PopUpBoxAnimator;
        string ToggleLensesText = "Click the button below to toggle your Paranormal Lenses on or off.";
        MessageText = ToggleLensesText;
        haveMessage = true;
    }

    

    // Function to determine if we entered a world just setting it to true if we have so we set the behavior for the intro audio.
    public void SetEnteredPokemonWorld()
    {

        EnteredPokemonWorld = true;
        AreWeInAWorld = EnteredPokemonWorld;
        
        CurrentWorld = "Pokemon World";

        // turn the enter pokemon world button off as we are now in the world and don't need to see it.
        // can add a brief pop up box or text saying welcome to the world of pokemon or soemthing later


        paraLensesScript.ActiveGameObjects.Add(PokemonWorld);

        PokemonWorld.SetActive(true);
        
        PokemonWorldButtonGameObject.SetActive(false);

        text_LeavePokemonWorldButtonGameObject.text = display_LeavePokemonWorld;

        popUp = PopUpSystem.instance;
        
        string AshOverThereText = "Over there! Is that Ash? There's something wrong with him.";
        MessageText = AshOverThereText;
        haveMessage = true;
    }

    // will have conditoins for this that once we have completed the world we can now leave probably will need to make another button
    // to pop up instead listing to two setter methods for the same button onclick
    public void SetLeftPokemonWorld()
    {
        EnteredPokemonWorld = false;
        CurrentWorld = "";
        AreWeInAWorld = EnteredPokemonWorld;
        text_PokemonWorldButtonGameObject.text = display_EnterPokemonWorld;

    }

    public bool GetEnteredPokemonWorld()
    {
        return EnteredPokemonWorld;
    }

    public string GetCurrentWorld()
    {
        return CurrentWorld;
    }

    public void DisplayMessage()
    {
        if (haveMessage == true)
        {

            popUp = PopUpSystem.instance;
            popUp.PopUp(MessageText);

        }
        haveMessage = false;
        MessageText = "";
    }
    public void SetHaveMessage(bool messageExists)
    {
        haveMessage = messageExists;
    }
    public void SetMessageText(string text)
    {
        MessageText = text;
    }

}
