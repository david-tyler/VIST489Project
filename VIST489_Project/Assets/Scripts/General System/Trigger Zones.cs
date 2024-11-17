using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    private const string AshTriggerZoneName = "Ash Trigger Zone";
    public Button PopUpBoxButton;

    // Singleton References
    PopUpSystem popUp;
    GameSystemBehavior gameSystem;
    ParaLensesButtonBehavior paraLensesScript;
    PokemonWorld pokeWorld;
    MessageBehavior messageBehavior;

    // index for ash's lines
    private int AshLinesIndex;
    public List<string> AshLines = new List<string>();

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

    private bool inMiddleHallway;
    private bool inBackHallway;
    private bool inBottomHallway;
    private bool inClassroom;

    private bool leftMiddleHallway;
    private bool leftBackHallway;
    private bool leftBottomHallway;
    private bool leftClassroom;



    private void Start()
    {
       
        AshLinesIndex = 0;
        placeGlyphLinesIndex = 0;
        inMiddleHallway = false;
        inBackHallway = false;
        inBottomHallway = false;
        inClassroom = true;

        leftMiddleHallway = false;
        leftBackHallway = false;
        leftBottomHallway = false;
        leftClassroom = false;


        
    }

    private void OnTriggerEnter(Collider other)
    {
        popUp = PopUpSystem.instance;
        gameSystem = GameSystemBehavior.instance;
        paraLensesScript = ParaLensesButtonBehavior.instance;
        pokeWorld = PokemonWorld.instance;
        messageBehavior = MessageBehavior.instance;

        string colliderTag = other.gameObject.tag;

        List<GameSystemBehavior.NarrativeEvent> narrativeEvents = new List<GameSystemBehavior.NarrativeEvent>
        {
            GameSystemBehavior.NarrativeEvent.EnteredPokemonWorld,
            GameSystemBehavior.NarrativeEvent.ParalensesOn
        };
        
        if (gameSystem.AreNarrativeEventsComplete(narrativeEvents))
        {
            
            if (gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.FreedAsh) == false)
            {
                switch (colliderTag)
                {
                    case "Ash Trigger Zone":
                        if (AshLinesIndex == 0)
                        {
                            PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                            popUp.PopUp(AshLines[AshLinesIndex]);
                            currentDialogue = AshLines;
                            
                            gameSystem.ToggleSkipButton(true);
                            AshLinesIndex += 1;
                            currentDialogueIndex = AshLinesIndex;
                            StartCoroutine(WaitForPressedSkip());
                            
                            
                        }
                        break;
                    case "Gate Trigger Zone":
                        messageBehavior.SetHaveMessage(true);
                        messageBehavior.SetMessageText(cantEnterGateLine);
                        break;         
                }
            }
            else if ( gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.FreedAsh) == true )
            {
                switch (colliderTag)
                {
                    case "Ash Trigger Zone":
                         // New Lines for Ash once you unlock the door but haven't freed Charizard yet.
                        AshLines.Clear();
                        AshLinesIndex = 0;
                        string lineOne = "Wow! You actually managed to unlock the door! Thanks for your help.";
                        string lineTwo = "But I can't leave without my Charizard and I have no idea where he is. Please help me find him.";
                        AshLines.Add(lineOne);
                        AshLines.Add(lineTwo);

                        currentDialogue = AshLines;
                        if (AshLinesIndex == 0)
                        {
                            PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                            popUp.PopUp(AshLines[AshLinesIndex]);
                            AshLinesIndex += 1;
                            currentDialogueIndex = AshLinesIndex;
                            StartCoroutine(WaitForPressedSkip());
                        }
                       
                        break;
                    case "Gate Trigger Zone":
                        messageBehavior.SetHaveMessage(true);
                        messageBehavior.SetMessageText(doTheGlyphLine);
                        break;
                    case "Glyph Trigger Zone":
                        messageBehavior.SetHaveMessage(true);
                        messageBehavior.SetMessageText(lookForGlyphPokeballs);

                        currentDialogue = placeGlyphLines;
                        PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                        popUp.PopUp(placeGlyphLines[placeGlyphLinesIndex]);
                        placeGlyphLinesIndex += 1;
                        currentDialogueIndex = placeGlyphLinesIndex;

                        StartCoroutine(WaitForPressedSkip());
                        break;
                    case "Charizard Zone":
                        pokeWorld.SetCanTapCharizard(true);
                        break;
                }
                
            }
            switch (colliderTag)
            {
                case "Middle Hallway":
                    inMiddleHallway = true;

                    if (leftMiddleHallway == true) // if you left the area then returned back to it
                    {
                        foreach (GameObject item in currentMiddleHallwayGameObjects)
                        {
                            item.SetActive(true);
                        }
                    }
                    leftMiddleHallway = false;
                    
                    break;

                case "Classroom":
                    inClassroom = true;

                    if (leftClassroom == true) // if you left the area then returned back to it
                    {
                        foreach (GameObject item in currentClassroomGameObjects)
                        {
                            item.SetActive(true);
                        }
                    }
                    leftClassroom = false;

                    break;

                case "Back Hallway":
                    inBackHallway = true;
                    if (leftBackHallway == true) // if you left the area then returned back to it
                    {
                        foreach (GameObject item in currentBackHallwayGameObjects)
                        {
                            item.SetActive(true);
                        }
                    }
                    leftBackHallway = false;
                    
                    break;
                    
                case "Bottom Hallway":
                    inBottomHallway = true;
                    if (leftBottomHallway == true) // if you left the area then returned back to it
                    {
                        foreach (GameObject item in currentBottomHallwayGameObjects)
                        {
                            item.SetActive(true);
                        }
                    }
                    leftBottomHallway = false;
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
            if ( pokeWorld.GetUnlockedDoor() == true )
            {
                switch (colliderTag)
                {
                    case "Charizard Zone":
                        pokeWorld.SetCanTapCharizard(false);
                        break;
                }
            }
            
            switch (colliderTag)
            {
                case "Middle Hallway":
                    inMiddleHallway = false;
                    foreach (GameObject item in currentMiddleHallwayGameObjects)
                    {
                        item.SetActive(false);
                    }
                    leftMiddleHallway = true;
                    break;
                case "Bottom Hallway":
                    inBottomHallway = false;
                    foreach (GameObject item in currentBottomHallwayGameObjects)
                    {
                        item.SetActive(false);
                    }
                    leftBottomHallway = true;
                    break;
                case "Back Hallway":
                    inBackHallway = false;
                    foreach (GameObject item in currentBackHallwayGameObjects)
                    {
                        item.SetActive(false);
                    }
                    leftBackHallway = true;

                    break;
                case "Classroom":
                    inClassroom = false;
                    foreach (GameObject item in currentClassroomGameObjects)
                    {
                        item.SetActive(false);
                    }
                    leftClassroom = true;
                    break;
            
            }
        }
    }

    public void ModifyLists(List<GameObject> objectsToRemove = null, bool leftBackhall = false, bool leftMiddlehall = false, bool leftClass = false,bool leftBottomhall = false)
    {
        if (objectsToRemove != null)
        {
            foreach (GameObject item in objectsToRemove)
            {
                if (middleHallwayGameObjects.Contains(item))
                {
                    middleHallwayGameObjects.Remove(item);
                }
                if (backHallwayGameObjects.Contains(item))
                {
                    backHallwayGameObjects.Remove(item);
                }
                if (bottomHallwayGameObjects.Contains(item))
                {
                    bottomHallwayGameObjects.Remove(item);
                }
                if (classroomGameObejcts.Contains(item))
                {
                    classroomGameObejcts.Remove(item);
                }
            }
        }
        
        foreach (GameObject item in middleHallwayGameObjects)
        {
            if (item.activeSelf == true && currentMiddleHallwayGameObjects.Contains(item) == false)
            {
                currentMiddleHallwayGameObjects.Add(item);
            }
        }

        if (inMiddleHallway == false)
        {
            foreach (GameObject item in currentMiddleHallwayGameObjects)
            {
                item.SetActive(false);
                leftMiddleHallway = leftMiddlehall;
            }
        }

        

        foreach (GameObject item in backHallwayGameObjects)
        {
            if (item.activeSelf == true && currentBackHallwayGameObjects.Contains(item) == false)
            {
                currentBackHallwayGameObjects.Add(item);
            }
        }
        if (inBackHallway == false)
        {
            foreach (GameObject item in currentBackHallwayGameObjects)
            {
                item.SetActive(false);
                leftBackHallway = leftBackhall;
            }
        }
        

        foreach (GameObject item in bottomHallwayGameObjects)
        {
            if (item.activeSelf == true && currentBottomHallwayGameObjects.Contains(item) == false)
            {
                currentBottomHallwayGameObjects.Add(item);
            }
        }
        
        if (inBottomHallway == false)
        {
            foreach (GameObject item in currentBottomHallwayGameObjects)
            {
                item.SetActive(false);
                leftBottomHallway = leftBottomhall;
            }
        }

        foreach (GameObject item in classroomGameObejcts)
        {
            if (item.activeSelf == true && currentClassroomGameObjects.Contains(item) == false)
            {
                currentClassroomGameObjects.Add(item);
            }
        }

        if (inClassroom == false)
        {
            foreach (GameObject item in currentClassroomGameObjects)
            {
                item.SetActive(false);
                leftClassroom = leftClass;
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

