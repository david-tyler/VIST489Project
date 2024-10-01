using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaLensesButtonBehavior : MonoBehaviour
{
    private bool isParaLensesOn = false;

    
    public GameObject ImageTargetEnterPkWorld;
    public GameObject ImageTargetPKAudio;
    public TMPro.TextMeshProUGUI ParaLensesText;



    private string text_EnableParaLenses = "Enable Paranormal Lenses";
    private string text_DisableParaLenses = "Disable Paranormal Lenses";


    public void Start()
    {
        isParaLensesOn = false;
    }

    public void ToggleParanormalLenses()
    {
        // Functionality to toggle the paralenses on/off
        isParaLensesOn = !isParaLensesOn;
        if (isParaLensesOn == true)
        {
            ParaLensesText.text = text_DisableParaLenses;
        }
        else if (isParaLensesOn == false)
        {
            ParaLensesText.text = text_EnableParaLenses;
        }

        
        ImageTargetPKAudio.SetActive(isParaLensesOn);
    }

    public bool getIsParaLensesOn()
    {
        return isParaLensesOn;
    }
    
}
