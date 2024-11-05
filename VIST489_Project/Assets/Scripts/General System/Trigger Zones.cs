using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UI;

public class TriggerZones : MonoBehaviour
{
    private const string AshTriggerZoneName = "Ash Trigger Zone";
    public Button PopUpBoxButton;

    // Singleton References
    PopUpSystem popUp;
    GameSystemBehavior gameSystem;
    ParaLensesButtonBehavior paraLensesScript;
    PokemonWorld pokeWorld;

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

    private void Start()
    {
       
        AshLinesIndex = 0;
        placeGlyphLinesIndex = 0;

        
    }

    private void OnTriggerEnter(Collider other)
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
                        gameSystem.SetHaveMessage(true);
                        gameSystem.SetMessageText(cantEnterGateLine);
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
                        gameSystem.SetHaveMessage(true);
                        gameSystem.SetMessageText(doTheGlyphLine);
                        break;
                    case "Glyph Trigger Zone":
                        gameSystem.SetHaveMessage(true);
                        gameSystem.SetMessageText(lookForGlyphPokeballs);

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
