using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    // Enum for narrative events for readability
    public enum NarrativeEvent
    {
        ParalensesOn,            // 0
        InIntroductionStage,     // 1
        EnteredPokemonWorld,     // 2
        CompletedMazeGotKey,     // 3
        FreedAsh,                // 4
        SolvedGlyph,             // 5
        SolvedPit,               // 6
        ReunitedAshWithCharizard,// 7
        AlternatePath            // 8
    }

    // Boolean array to track completion of narrative events
    private bool[] narrativeState = new bool[9];

    // Keep track of what interactable we have focused
    
    public GameObject ParaNormalLensesGameObject;
    public Button ParaNormalLensesButton;
    public Camera mainCamera;

    private ParaLensesButtonBehavior paraLensesScript;
    private PopUpSystem popUp;
    private Interactable focus;
    private AudioManager audioManagerScript;
    private TriggerZones triggerZonesScript;
    private Inventory inventoryScript;

    // Narration Audio Sources
    // --------------------------------------
    public AudioSource introAudio;
    public AudioSource FindAGatewayAudio;
    public AudioSource TapOnIt;
    public AudioSource ThatBook;
    // --------------------------------------

    // ******* PopUp Boxes
    // --------------------------------------
    public GameObject PopUpBox; // Our dialogue box
    public Animator PopUpBoxAnimator;

    public Animator MessageButtonAnimator;

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
    public GameObject SkipButtonGameObject;
    private Button SkipButton;
    // --------------------------------------


    // ******* Strings
    // --------------------------------------
    private string display_EnterPokemonWorld = "Enter Pokemon World";
    private string display_LeavePokemonWorld = "Leave Pokemon World";

    private string MessageText = "";
    // --------------------------------------


    // ******* Booleans for different conditions
    // --------------------------------------
    private bool gameStarted = false;
    // boolean to track if we have entered a world or not at least once for the introduction audio behavior

    private bool AreWeInAWorld = false;
    private bool haveMessage = false;
    private bool pressedSkip = false;
    private bool foundGateway = false;
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

    // All of the game objects we want to not be active once we start the game so like puzzles etc.
   

    

    public string backgroundMusicName;

     public List<GameObject> gameObjectsNotActive = new List<GameObject>();
    public List<Item> itemsReachedGlyph;
    public List<Item> itemsReachedPit;
    public List<Item> itemsReachedMaze;


    void Start()
    {
        ResetNarrativeState();

        paraLensesScript = ParaLensesButtonBehavior.instance;

        foreach (GameObject item in gameObjectsNotActive)
        {
            if(item != null)
            {
                item.SetActive(false);
            }
            
        }

        SkipButton = SkipButtonGameObject.GetComponent<Button>();
        

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

                        if (currentItem != null)
                        {
                            if (IsNarrativeEventComplete(NarrativeEvent.EnteredPokemonWorld))
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
                                    case "Old Key":
                                        SetFocus(interactable);
                                        break;
                                }
                            }

                        }
                        else
                        {
                            SetFocus(interactable);

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
                    if(currentItem != null)
                    {
                        if (IsNarrativeEventComplete(NarrativeEvent.EnteredPokemonWorld))
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
                                case "Old Key":
                                    SetFocus(interactable);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        SetFocus(interactable);
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

        // handling when the message notification pops up in the game
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

        AudioSource currentAudio = null;
        
        List<NarrativeEvent> narrativeEvents = new List<NarrativeEvent>
        {
            NarrativeEvent.ParalensesOn,
            NarrativeEvent.InIntroductionStage
        };

        if (AreNarrativeEventsComplete(narrativeEvents))
        {
            if ( FindAGatewayAudio.isPlaying == false && foundGateway == true )
            {
                currentAudio = ThatBook;
                ThatBook.Play();
            }
            else
            {
                currentAudio = FindAGatewayAudio;
                FindAGatewayAudio.Play();
            
            }
                

            if(currentAudio != null)
            {
                StartCoroutine(PlayingAudio(currentAudio));
                
            }
            
            
        }
        
    }

    // Handles playing an audio and enabling the skip button
    IEnumerator PlayingAudio(AudioSource currentAudio)
    {
        currentAudio.Play();

        ToggleSkipButton(true);
        
        // Wait for the duration of the audio or until skipped
        float elapsedTime = 0f;
        float audioLength = currentAudio.clip.length;

        // This while loop is to wait for the entire audio length to check if the player pressed skip
        while (elapsedTime < audioLength && pressedSkip == false)
        {
            yield return null;  // Wait for the next frame
            elapsedTime += Time.deltaTime;
        }
        
        if (pressedSkip == true)
        {
            SkipButtonInteraction(currentAudio);
        }

        ToggleSkipButton(false);
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
        narrativeState[(int)NarrativeEvent.InIntroductionStage] = true;

    }

    IEnumerator SetUpIntroduction()
    {
        introAudio.Play();

        ToggleSkipButton(true);
        
         // Wait for the duration of the audio or until skipped
        float elapsedTime = 0f;
        float audioLength = introAudio.clip.length;

        // Instead of waiting the entire duration at once, check every frame if the skip button was pressed
        while (elapsedTime < audioLength && pressedSkip == false)
        {
            yield return null;  // Wait for the next frame
            elapsedTime += Time.deltaTime;
        }
        
       
        if (pressedSkip == true)
        {
            SkipButtonInteraction(introAudio);
        }

        ToggleSkipButton(false);
        

        // reset pressedSkip for other uses with different audios
        pressedSkip = false;

        ParaNormalLensesGameObject.SetActive(true);

        // Follow this as a guide to modify pop up boxes where you want
        popUp = PopUpSystem.instance;
        popUp.popUpBox = PopUpBox;
        popUp.popUpText = PopUpBoxText;
        popUp.animator = PopUpBoxAnimator;
        string ToggleLensesText = "Click the button below to toggle your Paranormal Lenses on or off.";
        MessageText = ToggleLensesText;
        SetHaveMessage(true);
    }

    public void SkipButtonInteraction(AudioSource currentAudio = null, bool inDialogueSequence = false)
    {
        
        if (inDialogueSequence == true) // if we are in a dialogue sequence we want to skip the sequence
        {
            // skip the current dialogue sequence
            // haven't found use for this am only since the only dialogue sequence is in a coroutine
            // and i can't call yield ... here in a void method if i call this method in a coroutine.
        }
        else if (currentAudio != null) // if we are currently playing an audio we want to skip that
        {
            currentAudio.Stop();
        }
        pressedSkip = false;
    }

    public void ToggleSkipButton(bool activate)
    {
        if (activate == true)
        {
            SkipButtonGameObject.SetActive(true);
            SkipButton.onClick.AddListener(SkipButtonListener);
        }
        else if(activate == false)
        {
            SkipButton.onClick.RemoveListener(SkipButtonListener);
            SkipButtonGameObject.SetActive(false);
            pressedSkip = false;
        }
    }

    public void SkipButtonListener()
    {
        pressedSkip = true;
    }

    // Function to determine if we entered a world just setting it to true if we have so we set the behavior for the intro audio.
    public void SetEnteredPokemonWorld()
    {
        audioManagerScript = AudioManager.instance;
        audioManagerScript.PlayEventSound(backgroundMusicName);

        SetNarrativeEvent(NarrativeEvent.EnteredPokemonWorld, true);
        SetNarrativeEvent(NarrativeEvent.InIntroductionStage, false);
        triggerZonesScript = TriggerZones.instance;


        // turn the enter pokemon world button off as we are now in the world and don't need to see it.
        // can add a brief pop up box or text saying welcome to the world of pokemon or soemthing later


        paraLensesScript.ActiveGameObjects.Add(PokemonWorld);

        PokemonWorld.SetActive(true);
        
        PokemonWorldButtonGameObject.SetActive(false);

        text_LeavePokemonWorldButtonGameObject.text = display_LeavePokemonWorld;

        popUp = PopUpSystem.instance;
        
        string AshOverThereText = "Over there! Is that Ash? There's something wrong with him.";
        MessageText = AshOverThereText;
        SetHaveMessage(true);

        triggerZonesScript.ModifyLists();
    }

    // will have conditoins for this that once we have completed the world we can now leave probably will need to make another button
    // to pop up instead listing to two setter methods for the same button onclick
    public void SetLeftPokemonWorld()
    {
        SetNarrativeEvent(NarrativeEvent.EnteredPokemonWorld, false);

        AreWeInAWorld = narrativeState[(int)NarrativeEvent.EnteredPokemonWorld];
        text_PokemonWorldButtonGameObject.text = display_EnterPokemonWorld;

    }

    public bool getPressedSkip()
    {
        return pressedSkip;
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
        MessageButtonAnimator.SetTrigger("Shake");
        Handheld.Vibrate(); // vibrate phone whenever player gets a message
    }
    public void SetMessageText(string text)
    {
        MessageText = text;
    }

    public void SetFoundGateway()
    {
        foundGateway = true;
        PlayAudios();
    }

     public void SetNarrativeEvent(NarrativeEvent narrativeEvent, bool status)
    {
        int index = (int)narrativeEvent;
        if (index >= 0 && index < narrativeState.Length)
        {
            narrativeState[index] = status;
        }
    }

    // Check if an event is completed
    public bool IsNarrativeEventComplete(NarrativeEvent narrativeEvent)
    {
        int index = (int)narrativeEvent;
        if (index >= 0 && index < narrativeState.Length)
        {
            return narrativeState[index];
        }
        return false;
    }


    // rather than having multiple && in an if statement just create a list of each event we want to be true and call this function to check if they're all true
    public bool AreNarrativeEventsComplete(List<NarrativeEvent> narrativeEvents)
    {
        int index;
        foreach (NarrativeEvent narrativeEvent in narrativeEvents)
        {
            index = (int)narrativeEvent;

            if (narrativeState[index] == false)
            {
                return false;
            }
        }
        return true;
    }

    // Example method to check if all events are completed
    public bool IsNarrativeComplete()
    {
        foreach (bool state in narrativeState)
        {
            if (!state) return false;
        }
        return true;
    }
    public void ResetNarrativeState()
    {
        for (int i = 0; i < narrativeState.Length; i++)
        {
            narrativeState[i] = false;
        }
    }

    public void SetOverallNarrativeState(List<bool> currentState)
    {
        if (currentState.Count != narrativeState.Length)
        {
            Debug.LogWarning("Incorrect size of booleans for narrative");

            return;
        }
        for (int i = 0; i < narrativeState.Length; i++)
        {
            narrativeState[i] = currentState[i];
            
        }
    }

    public bool CheckNarrativeEvents(Dictionary<NarrativeEvent, bool> expectedEvents)
    {
        foreach (KeyValuePair <NarrativeEvent, bool> kvp in expectedEvents)
        {
            NarrativeEvent narrativeEvent = kvp.Key;
            bool expectedState = kvp.Value;

            int index = (int)narrativeEvent;

            if (index >= 0 && index < narrativeState.Length)
            {
                if (narrativeState[index] != expectedState)
                {
                    return false;
                }
            }
            else
            {
                Debug.LogWarning($"Invalid Narrative Event: {narrativeEvent}, checking at invalid index: {index}");
                return false;
            }
        }
        return true;
    }

    public void SetUpGlyphNarrativeState()
    {
        inventoryScript = Inventory.instance;
        List<bool> currentState = new List<bool>();
        currentState.Add(IsNarrativeEventComplete(NarrativeEvent.ParalensesOn));
        currentState.Add(false);
        currentState.Add(true);
        currentState.Add(true);
        currentState.Add(true);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        SetOverallNarrativeState(currentState);

        inventoryScript.Clear();
        if (itemsReachedGlyph.Any())
        {
            foreach (Item item in itemsReachedGlyph)
            {
                inventoryScript.Add(item);
            }
        }
        

    }

    public void SetUpMazeNarrativeState()
    {
        inventoryScript = Inventory.instance;
        List<bool> currentState = new List<bool>();
        currentState.Add(IsNarrativeEventComplete(NarrativeEvent.ParalensesOn));
        currentState.Add(false);
        currentState.Add(true);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        SetOverallNarrativeState(currentState);

        inventoryScript.Clear();
        if (itemsReachedMaze.Any())
        {
            foreach (Item item in itemsReachedMaze)
            {
                inventoryScript.Add(item);
            }
        }
        

    }
    public void SetUpPitNarrativeState()
    {
        inventoryScript = Inventory.instance;
        List<bool> currentState = new List<bool>();
        currentState.Add(IsNarrativeEventComplete(NarrativeEvent.ParalensesOn));
        currentState.Add(false);
        currentState.Add(true);
        currentState.Add(true);
        currentState.Add(true);
        currentState.Add(true);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        SetOverallNarrativeState(currentState);

        inventoryScript.Clear();
        if (itemsReachedPit.Any())
        {
            foreach (Item item in itemsReachedPit)
            {
                inventoryScript.Add(item);
            }
        }        

    }
}
