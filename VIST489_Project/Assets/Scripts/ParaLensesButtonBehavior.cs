using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaLensesButtonBehavior : MonoBehaviour
{
    private bool isParaLensesOn = false;
    private bool inPokemonWorld = false;
    private string disableLenses = "Disable Paranormal Lenses";
    private string enableLenses = "Enable Paranormal Lenses";
    private string LeavePKWorld = "Leave Pokemon World";
    private string enterPKWorld = "Enter Pokemon World";


    public GameObject enterPkWorldButton;
    public GameObject ImageTargetCharSitting;
    public GameObject ImageTargetPKAudio;
    public TMPro.TextMeshProUGUI ParaLenses;
    public TMPro.TextMeshProUGUI PkWorldBttnText;
    private void Start()
    {
        ImageTargetCharSitting.SetActive(false);
        ImageTargetPKAudio.SetActive(false);
    }


    public void Start()
    {
        isParaLensesOn = false;
    }

    public void ShowEnterPkWorldButton()
    {
        isParaLensesOn = !isParaLensesOn;

        if (isParaLensesOn)
        {
            ParaLenses.text = disableLenses;
        }
        else
        {
            ParaLenses.text = enableLenses;
        }


        enterPkWorldButton.SetActive(isParaLensesOn);
        
    }

    public void EnterPKWorldButtonClicked()
    {
        inPokemonWorld = !inPokemonWorld;

        if (inPokemonWorld)
        {
            PkWorldBttnText.text = LeavePKWorld;
        }
        else
        {
            PkWorldBttnText.text = enterPKWorld;
        }

        ImageTargetCharSitting.SetActive(inPokemonWorld);
        ImageTargetPKAudio.SetActive(inPokemonWorld);
    }


    public bool getIsParaLensesOn()
    {
        return isParaLensesOn;
    }
    
}
