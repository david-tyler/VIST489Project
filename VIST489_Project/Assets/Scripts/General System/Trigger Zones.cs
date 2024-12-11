using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.TextCore;
using UnityEngine.UI;

public class TriggerZones : MonoBehaviour
{
    #region Singleton
    public static TriggerZones instance;
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
    public Button PopUpBoxButton;

    // Singleton References
    PopUpSystem popUp;
    GameSystemBehavior gameSystem;
    ParaLensesButtonBehavior paraLensesScript;
    PokemonWorld pokeWorld;
    MessageBehavior messageBehavior;
    MazeBehavior mazeBehaviorScript;

    public List<string> placeGlyphLines = new List<string>();

    private List<string> currentDialogue;
    private int currentDialogueIndex;
    
    private int placeGlyphLinesIndex;
    
   

    public string cantEnterGateLine = "It seems like we can't enter this area right now. We need to find the key and free Ash first.";
    public string doTheGlyphLine = "The gate is locked. Maybe we can open it by solving that glyph on the wall to your right.";
    public string lookForGlyphPokeballs = "Those colors on the glyph seem important. Maybe we need to find objects with those colors, your map should help with this.";

    public List<GameObject> middleHallwayGameObjects;
    public List<GameObject> backHallwayGameObjects;
    public List<GameObject> bottomHallwayGameObjects;
    public List<GameObject> classroomGameObejcts;

    private List<GameObject> currentMiddleHallwayGameObjects = new List<GameObject>();
    private List<GameObject> currentBackHallwayGameObjects = new List<GameObject>();
    private List<GameObject> currentBottomHallwayGameObjects = new List<GameObject>();
    private List<GameObject> currentClassroomGameObjects = new List<GameObject>();

    private bool movedCharizard = false;


    private void Start()
    {
       
        placeGlyphLinesIndex = 0;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        popUp = PopUpSystem.instance;
        gameSystem = GameSystemBehavior.instance;
        paraLensesScript = ParaLensesButtonBehavior.instance;
        pokeWorld = PokemonWorld.instance;
        messageBehavior = MessageBehavior.instance;
        mazeBehaviorScript = MazeBehavior.instance;
        

        string colliderTag = other.gameObject.tag;

        List<GameSystemBehavior.NarrativeEvent> narrativeEvents = new List<GameSystemBehavior.NarrativeEvent>
        {
            GameSystemBehavior.NarrativeEvent.EnteredPokemonWorld,
            GameSystemBehavior.NarrativeEvent.ParalensesOn
        };
        
        if (gameSystem.AreNarrativeEventsComplete(narrativeEvents))
        {
            if (gameSystem.GetCurrentState() == GameSystemBehavior.NarrativeEvent.SolvedPit)
            {
                if (other.tag == "Middle Hallway" && movedCharizard == false)
                {
                    foreach (GameObject item in backHallwayGameObjects)
                    {
                        if (item.tag == "Charizard")
                        {
                            backHallwayGameObjects.Remove(item);
                            item.SetActive(true);
                            break;
                        }
                            

                    }
                    pokeWorld = PokemonWorld.instance;
                    StartCoroutine(pokeWorld.MoveCharizardSequence());
                    movedCharizard = true;
                    
                }
            }
            if (gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.FreedAsh) == false)
            {
                switch (colliderTag)
                {
                    
                    case "Gate Trigger Zone":
                        messageBehavior.SetHaveMessage(true);
                        messageBehavior.SetMessageText(cantEnterGateLine);
                        break;         
                }
            }
            else if ( gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.FreedAsh) == true  && gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.SolvedGlyph) == false)
            {
                switch (colliderTag)
                {
                    case "Gate Trigger Zone":
                        messageBehavior.SetHaveMessage(true);
                        messageBehavior.SetMessageText(doTheGlyphLine);
                        break;
                    case "Glyph Trigger Zone":
                    
                        messageBehavior.SetHaveMessage(true);
                        messageBehavior.SetMessageText(lookForGlyphPokeballs);
                        placeGlyphLinesIndex = 0;
                        currentDialogue = placeGlyphLines;
                        PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                        popUp.PopUp(placeGlyphLines[placeGlyphLinesIndex]);
                        gameSystem.ToggleSkipButton(true);
                        placeGlyphLinesIndex += 1;
                        currentDialogueIndex = placeGlyphLinesIndex;

                        StartCoroutine(WaitForPressedSkip());
                        break;
                    
                }
                
            }
            else if (gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.FreedAsh) == true  && gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.SolvedGlyph) == true)
            {
                switch (colliderTag)
                {
                    case "Charizard Zone":
                        pokeWorld.SetCanTapCharizard(true);
                        break;
                }
                    
            }


            
            
        }
        

    }

    void OnTriggerStay(Collider other)
    {
        popUp = PopUpSystem.instance;
        gameSystem = GameSystemBehavior.instance;
        paraLensesScript = ParaLensesButtonBehavior.instance;
        pokeWorld = PokemonWorld.instance;
        messageBehavior = MessageBehavior.instance;
        mazeBehaviorScript = MazeBehavior.instance;
        

        string colliderTag = other.gameObject.tag;

        List<GameSystemBehavior.NarrativeEvent> narrativeEvents = new List<GameSystemBehavior.NarrativeEvent>
        {
            GameSystemBehavior.NarrativeEvent.EnteredPokemonWorld,
            GameSystemBehavior.NarrativeEvent.ParalensesOn
        };
        if (gameSystem.AreNarrativeEventsComplete(narrativeEvents))
        {
            switch (colliderTag)
            {
                case "Middle Hallway":

                    foreach (GameObject item in middleHallwayGameObjects)
                    {
                        if (item != null)
                        {
                            if (pokeWorld.objectsToSetActiveAfterDoor.Contains(item) == true)
                            {
                                if (pokeWorld.GetUnlockedDoor() == false)
                                {
                                    continue;
                                }
                                ItemPickup itemPickupScript = item.GetComponentInChildren<ItemPickup>();
                                if (itemPickupScript != null)
                                {
                                    // if you already picked up the item set it to false. Need to add this cause of debug buttons 
                                    // moving you in the narrative and collecting items
                                    if (itemPickupScript.GetCompletedPickUp() == true)
                                    {
                                        item.SetActive(false);
                                    }
                                    else
                                    {
                                        item.SetActive(true);
                                    }
                                    continue;
                                }
                                item.SetActive(true);
                            }
                            else
                            {
                                item.SetActive(true);
                            }

                        }

                    }

                    break;

                case "Classroom":

                    foreach (GameObject item in classroomGameObejcts)
                    {
                        if (item != null)
                        {
                            if (pokeWorld.objectsToSetActiveAfterDoor.Contains(item) == true)
                            {
                                if (pokeWorld.GetUnlockedDoor() == false)
                                {
                                    continue;
                                }
                                ItemPickup itemPickupScript = item.GetComponentInChildren<ItemPickup>();
                                if (itemPickupScript != null)
                                {
                                    // if you already picked up the item set it to false. Need to add this cause of debug buttons 
                                    // moving you in the narrative and collecting items
                                    if (itemPickupScript.GetCompletedPickUp() == true)
                                    {
                                        item.SetActive(false);
                                    }
                                    else
                                    {
                                        item.SetActive(true);
                                    }
                                    continue;
                                }
                                item.SetActive(true);
                            }
                            else
                            {
                                item.SetActive(true);
                            }

                        }

                    }


                    break;

                case "Back Hallway":
                    pokeWorld = PokemonWorld.instance;

                    foreach (GameObject item in backHallwayGameObjects)
                    {
                        if (item != null)
                        {
                            if (item.tag == "Gate")
                            {
                                if (pokeWorld.GetCanTapCharizard() == true)
                                {
                                    item.SetActive(false);
                                    continue;
                                }
                                item.SetActive(true);
                            }
                            else if (item.tag == "Pit" || item.tag == "Charizard")
                            {

                                if (pokeWorld.GetCanTapCharizard() == false)
                                {

                                    item.SetActive(false);
                                }
                                else
                                {
                                    item.SetActive(true);
                                }
                                continue;
                            }
                            else if (pokeWorld.objectsToSetActiveAfterDoor.Contains(item) == true)
                            {
                                if (pokeWorld.GetUnlockedDoor() == false)
                                {
                                    continue;
                                }
                                ItemPickup itemPickupScript = item.GetComponentInChildren<ItemPickup>();
                                if (itemPickupScript != null)
                                {
                                    // if you already picked up the item set it to false. Need to add this cause of debug buttons 
                                    // moving you in the narrative and collecting items
                                    if (itemPickupScript.GetCompletedPickUp() == true)
                                    {
                                        item.SetActive(false);
                                    }
                                    else
                                    {
                                        item.SetActive(true);
                                    }
                                    continue;
                                }

                                item.SetActive(true);
                            }
                            else
                            {
                                item.SetActive(true);
                            }
                        }


                    }


                    break;

                case "Bottom Hallway":

                    foreach (GameObject item in bottomHallwayGameObjects)
                    {
                        if (item != null)
                        {
                            if (item.tag == "Left Klefki" || item.tag == "Right Klefki")
                            {
                                if (mazeBehaviorScript.getMazeStartedBool() == false)
                                {
                                    Debug.Log("Here " + mazeBehaviorScript.getMazeStartedBool());
                                    continue;
                                }
                                else
                                {
                                    item.SetActive(true);
                                }
                            }
                            else if (pokeWorld.objectsToSetActiveAfterDoor.Contains(item) == true)
                            {
                                if (pokeWorld.GetUnlockedDoor() == false)
                                {
                                    continue;
                                }
                                ItemPickup itemPickupScript = item.GetComponentInChildren<ItemPickup>();
                                if (itemPickupScript != null)
                                {
                                    // if you already picked up the item set it to false. Need to add this cause of debug buttons 
                                    // moving you in the narrative and collecting items
                                    if (itemPickupScript.GetCompletedPickUp() == true)
                                    {
                                        item.SetActive(false);
                                    }
                                    else
                                    {
                                        item.SetActive(true);
                                    }
                                    continue;
                                }
                                item.SetActive(true);
                            }
                            else
                            {
                                item.SetActive(true);
                            }
                        }


                    }

                    break;
            }
        }
    }

    IEnumerator WaitForPressedSkip()
    {
        gameSystem = GameSystemBehavior.instance;
        while (currentDialogueIndex <= currentDialogue.Count)
        {
            Debug.Log("Waiting To Press Skip");
            if (gameSystem.getPressedSkip() == true)
            {
                PopUpBoxButton.onClick.RemoveAllListeners();
                popUp.RemovePopUp();
                gameSystem.ToggleSkipButton(false);
                currentDialogueIndex = 0;
                

                // skip the dialogue so break from the coroutine
                // gameSystem.SkipButtonInteraction(null, true);
                
                yield break;

                
            }
            yield return null;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        popUp = PopUpSystem.instance;
        gameSystem = GameSystemBehavior.instance;
        paraLensesScript = ParaLensesButtonBehavior.instance;
        pokeWorld = PokemonWorld.instance;

        string colliderTag = other.gameObject.tag;

      
        List<GameSystemBehavior.NarrativeEvent> narrativeEvents = new List<GameSystemBehavior.NarrativeEvent>
        {
            GameSystemBehavior.NarrativeEvent.EnteredPokemonWorld,
            GameSystemBehavior.NarrativeEvent.ParalensesOn
        };
        
        if (gameSystem.AreNarrativeEventsComplete(narrativeEvents))
        {
            switch (colliderTag)
            {
                case "Middle Hallway":
                   
                    foreach (GameObject item in middleHallwayGameObjects)
                    {
                        if (item != null)
                        {
                            item.SetActive(false);
                        }
                        
                    }
                    
                    break;
                case "Bottom Hallway":
                   
                    foreach (GameObject item in bottomHallwayGameObjects)
                    {
                        if (item != null)
                        {
                            item.SetActive(false);
                        }
                        
                    }
                   
                    break;
                case "Back Hallway":
                    
                    foreach (GameObject item in backHallwayGameObjects)
                    {
                        if (item != null)
                        {
                            item.SetActive(false);
                        }
                        
                    }
                    

                    break;
                case "Classroom":
                   
                    foreach (GameObject item in classroomGameObejcts)
                    {
                        if (item != null)
                        {
                            item.SetActive(false);
                        }
                        
                    }
                    
                    break;
            
            }
        }
    }


    private void DisplayNextLines()
    {
        if (currentDialogueIndex >= currentDialogue.Count)
        {
            
            PopUpBoxButton.onClick.RemoveAllListeners();
            gameSystem.ToggleSkipButton(false);

            return;
        }
        else
        {
            
            StartCoroutine(WaitBetweenLines());
        }
        
    }

    IEnumerator WaitBetweenLines()
    {
        
        yield return new WaitForSeconds(1.0f);
        if (currentDialogueIndex == 0)
        {
            // if user presses skip before the next pop up appears then this ensures that next pop up won't appear
            // bug I found when trying to press skip like this
            yield break;
        }
        popUp = PopUpSystem.instance;
        popUp.PopUp(currentDialogue[currentDialogueIndex]);
        currentDialogueIndex += 1;
    }


}

