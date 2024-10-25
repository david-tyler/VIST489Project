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
    private List<string> AshLines = new List<string>();

    private string AshLine1 = "Oh No! Where did he go!?";
    private string AshLine2 = "I'm trapped in this room and I lost my Charizard. Will you please help me escape?";
    private string AshLine3 = "Thank you! That door is locked and the key isn't in this room, I have no clue where it could possibly be.";
    private string AshLine4 = "";

    public string cantEnterGateLine = "It seems like we can't enter this area right now. We need to find the key and free Ash first.";
    public string doTheGlyphLine = "The gate is locked. Maybe we can open it by solving that glyph on the wall to your right.";
    public string lookForGlyphPokeballs = "Those colors on the glyph seem important. Maybe we need to find objects with those colors, your map should help with this.";

    private void Start()
    {
        AshLines.Add(AshLine1);
        AshLines.Add(AshLine2);
        AshLines.Add(AshLine3);
        AshLinesIndex = 0;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        popUp = PopUpSystem.instance;
        gameSystem = GameSystemBehavior.instance;
        paraLensesScript = ParaLensesButtonBehavior.instance;
        pokeWorld = PokemonWorld.instance;

        string colliderTag = other.gameObject.tag;

        if (gameSystem.GetEnteredPokemonWorld() == true && paraLensesScript.getIsParaLensesOn())
        {
            if ( pokeWorld.GetUnlockedDoor() == false )
            {
                switch (colliderTag)
                {
                    case "Ash Trigger Zone":
                        if (AshLinesIndex == 0)
                        {
                            PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                            popUp.PopUp(AshLines[AshLinesIndex]);
                            gameSystem.ToggleSkipButton(true);
                            AshLinesIndex += 1;
                            StartCoroutine(WaitForPressedSkip());
                            
                            
                        }
                        break;
                    case "Gate Trigger Zone":
                        gameSystem.SetHaveMessage(true);
                        gameSystem.SetMessageText(cantEnterGateLine);
                        break;
                }
            }
            else if ( pokeWorld.GetUnlockedDoor() == true )
            {
                switch (colliderTag)
                {
                    case "Ash Trigger Zone":
                         // New Lines for Ash once you unlock the door but haven't freed Charizard yet.
                        AshLines.Clear();
                        AshLinesIndex = 0;
                        AshLine1 = "Wow! You actually managed to unlock the door! Thanks for your help.";
                        AshLine2 = "But I can't leave without my Charizard and I have no idea where he is. Please help me find him.";
                        AshLines.Add(AshLine1);
                        AshLines.Add(AshLine2);

                        if (AshLinesIndex == 0)
                        {
                            PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                            popUp.PopUp(AshLines[AshLinesIndex]);
                            AshLinesIndex += 1;
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
        while (AshLinesIndex <= AshLines.Count)
        {
            Debug.Log("Waiting To Press Skip");
            if (gameSystem.getPressedSkip() == true)
            {
                PopUpBoxButton.onClick.RemoveAllListeners();
                popUp.RemovePopUp();
                gameSystem.ToggleSkipButton(false);
                AshLinesIndex = 0;
                

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

        if (gameSystem.GetEnteredPokemonWorld() == true && paraLensesScript.getIsParaLensesOn())
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
        if (AshLinesIndex >= AshLines.Count)
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
        if (AshLinesIndex == 0)
        {
            // if user presses skip before the next pop up appears then this ensures that next pop up won't appear
            // bug I found when trying to press skip like this
            yield break;
        }
        popUp = PopUpSystem.instance;
        popUp.PopUp(AshLines[AshLinesIndex]);
        AshLinesIndex += 1;
    }


}

