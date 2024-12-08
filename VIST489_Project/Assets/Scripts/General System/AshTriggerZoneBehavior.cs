using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AshTriggerZoneBehavior : MonoBehaviour
{

    public Button PopUpBoxButton;

    // Singleton References
    PopUpSystem popUp;
    GameSystemBehavior gameSystem;
    ParaLensesButtonBehavior paraLensesScript;
    PokemonWorld pokeWorld;
    MessageBehavior messageBehavior;

    private int AshLinesIndex;
    public List<string> AshLines = new List<string>();

    private bool triggeredAshDialogue = false;
    private int currentDialogueIndex;

    private List<string> currentDialogue;

    void OnTriggerEnter(Collider other)
    {
        gameSystem = GameSystemBehavior.instance;
        popUp = PopUpSystem.instance;

        if (other.tag == "Player")
        {
            if (gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.FreedAsh) == false)
            {
                if (AshLinesIndex == 0)
                {
                    if (triggeredAshDialogue == false)
                    {
                        PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                        popUp.PopUp(AshLines[AshLinesIndex]);
                        currentDialogue = AshLines;

                        gameSystem.ToggleSkipButton(true);
                        AshLinesIndex += 1;
                        currentDialogueIndex = AshLinesIndex;
                        StartCoroutine(WaitForPressedSkip());
                        triggeredAshDialogue = true;
                    }



                }
            }
            else if (gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.FreedAsh) == true && gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.SolvedGlyph) == false)
            {
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
                    if (triggeredAshDialogue == false)
                    {
                        PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                        popUp.PopUp(AshLines[AshLinesIndex]);
                        AshLinesIndex += 1;
                        currentDialogueIndex = AshLinesIndex;
                        StartCoroutine(WaitForPressedSkip());
                        triggeredAshDialogue = true;
                    }
                    
                }
            }
            else if (gameSystem.IsNarrativeEventComplete(GameSystemBehavior.NarrativeEvent.SolvedPit))
            {
                 // New Lines for Ash once you unlock the door but haven't freed Charizard yet.
                AshLines.Clear();
                AshLinesIndex = 0;
                string lineOne = "Thank you so much for bringing back my Charizard.";
                string lineTwo = "I don't know how I can ever repay you but if you ever need anything please let me know.";
                AshLines.Add(lineOne);
                AshLines.Add(lineTwo);

                currentDialogue = AshLines;
                if (AshLinesIndex == 0)
                {
                    if (triggeredAshDialogue == false)
                    {
                        PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                        popUp.PopUp(AshLines[AshLinesIndex]);
                        AshLinesIndex += 1;
                        currentDialogueIndex = AshLinesIndex;
                        StartCoroutine(WaitForPressedSkip());
                        triggeredAshDialogue = true;
                    }
                }
            }
        }
    }

    private void DisplayNextLines()
    {
        gameSystem = GameSystemBehavior.instance;
        if (currentDialogueIndex >= currentDialogue.Count)
        {
            
            PopUpBoxButton.onClick.RemoveAllListeners();
            gameSystem.ToggleSkipButton(false);

            return;
        }
        else
        {
            
            StartCoroutine(WaitBetweenLines());
            gameSystem.ToggleSkipButton(true);

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

    IEnumerator WaitForPressedSkip()
    {
        gameSystem = GameSystemBehavior.instance;
        while (currentDialogueIndex <= currentDialogue.Count)
        {
            if (gameSystem.getPressedSkip() == true)
            {
                PopUpBoxButton.onClick.RemoveAllListeners();
                popUp.RemovePopUp();
                gameSystem.ToggleSkipButton(false);
                currentDialogueIndex = 0;
                

                // skip the dialogue so break from the coroutine
                // gameSystem.SkipButtonInteraction(null, true);
                triggeredAshDialogue = false;
                yield break;

                
            }
            yield return null;
        }
        triggeredAshDialogue = false;
        
    }

    
}
