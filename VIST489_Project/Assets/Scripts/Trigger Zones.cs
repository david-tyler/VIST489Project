using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TriggerZones : MonoBehaviour
{
    public string AshTriggerZoneName = "Ash Trigger Zone";
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
            if ( pokeWorld.GetUnlockedDoor() == false)
            {
                Debug.Log(colliderTag);
                if (colliderTag == AshTriggerZoneName)
                {
                    if (AshLinesIndex == 0)
                    {
                        PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                        popUp.PopUp(AshLines[AshLinesIndex]);
                        AshLinesIndex += 1;
                        
                        
                    }
                    
                }
                else if (colliderTag == "Gate Trigger Zone")
                {
                    gameSystem.SetHaveMessage(true);
                    gameSystem.SetMessageText(cantEnterGateLine);

                }
            }
            else if ( pokeWorld.GetUnlockedDoor() == true )
            {
                
                if (colliderTag == AshTriggerZoneName)
                {
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
                        
                        
                    }
                    
                }
                else if (colliderTag == "Gate Trigger Zone")
                {
                    gameSystem.SetHaveMessage(true);
                    gameSystem.SetMessageText(doTheGlyphLine);

                }
                else if (colliderTag == "Glyph Trigger Zone")
                {
                    gameSystem.SetHaveMessage(true);
                    gameSystem.SetMessageText(lookForGlyphPokeballs);

                }
                else if (colliderTag == "Charizard Zone")
                {
                    pokeWorld.SetCanTapCharizard(true);
                    

                }
            }
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
                if (colliderTag == "Charizard Zone")
                {
                    pokeWorld.SetCanTapCharizard(false);
                }
            }
        }
    }

    private void DisplayNextLines()
    {
        if (AshLinesIndex >= AshLines.Count)
        {
            
            PopUpBoxButton.onClick.RemoveAllListeners();
            return;
        }
        else
        {
            
            StartCoroutine(WaitBetweenLines());
        }
        
    }

    IEnumerator WaitBetweenLines()
    {
        yield return new WaitForSeconds(1.5f);
        popUp = PopUpSystem.instance;
        popUp.PopUp(AshLines[AshLinesIndex]);
        AshLinesIndex += 1;
    }


}

