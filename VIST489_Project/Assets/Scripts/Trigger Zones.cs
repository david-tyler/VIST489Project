using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerZones : MonoBehaviour
{
    public string AshTriggerZoneName = "Ash Trigger Zone";
    public Button PopUpBoxButton;

    PopUpSystem popUp;

    // index for ash's lines
    private int AshLinesIndex = 0;
    private List<string> AshLines = new List<string>();

    public string AshLine1 = "Oh No! Where did he go!?";
    public string AshLine2 = "I'm trapped in this room and I lost my Charizard. Will you please help me escape?";
    public string AshLine3;

    private bool pressedContinue = false;

    private void Start()
    {
        AshLines.Add(AshLine1);
        AshLines.Add(AshLine2);
    }

    private void OnTriggerEnter(Collider other)
    {
        popUp = PopUpSystem.instance;

        string colliderName = other.gameObject.tag;
        
        if (colliderName == AshTriggerZoneName)
        {
            
            if (AshLinesIndex == 0)
            {
                PopUpBoxButton.onClick.AddListener(SetPressedContinue);
                popUp.PopUp(AshLines[AshLinesIndex]);
                AshDialogue();
                
            }
            
            
        }

    }
    private void AshDialogue()
    {

        
        while (AshLinesIndex < AshLines.Count)
        {
            Debug.Log(AshLinesIndex);
            
            if (pressedContinue)
            {
                StartCoroutine(DisplayNextLines(AshLines[AshLinesIndex])); ;
            }
            pressedContinue = false;
            
            AshLinesIndex += 1;
        }

        // reset index to 0 so if we reach Ash again
        AshLinesIndex = 0;
        PopUpBoxButton.onClick.RemoveAllListeners();

    }
    private void SetPressedContinue()
    {
        pressedContinue = true;
    }

    IEnumerator DisplayNextLines(string line)
    {
        yield return new WaitForSeconds(1.0f);

        popUp = PopUpSystem.instance;
        popUp.PopUp(line);

        
    }


}

