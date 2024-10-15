using System.Collections;
using System.Collections.Generic;
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

    public string AshLine1 = "Oh No! Where did he go!?";
    public string AshLine2 = "I'm trapped in this room and I lost my Charizard. Will you please help me escape?";
    public string AshLine3 = "Part 2 I'm trapped in this room and I lost my Charizard. Will you please help me escape?";
    public string AshLine4 = "";

    private void Start()
    {
        AshLines.Add(AshLine1);
        AshLines.Add(AshLine2);
        AshLines.Add(AshLine3);
        AshLines.Add(AshLine4);
        AshLinesIndex = 0;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        popUp = PopUpSystem.instance;
        gameSystem = GameSystemBehavior.instance;
        paraLensesScript = ParaLensesButtonBehavior.instance;
        pokeWorld = PokemonWorld.instance;

        if (gameSystem.GetEnteredPokemonWorld() == true && paraLensesScript.getIsParaLensesOn() && pokeWorld.GetUnlockedDoor() == false)
        {
            string colliderName = other.gameObject.tag;
        
            if (colliderName == AshTriggerZoneName)
            {
                
                if (AshLinesIndex == 0)
                {
                    PopUpBoxButton.onClick.AddListener(DisplayNextLines);
                    popUp.PopUp(AshLines[AshLinesIndex]);
                    AshLinesIndex += 1;
                    
                    
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

