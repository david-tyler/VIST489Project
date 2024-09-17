using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaLensesButtonBehavior : MonoBehaviour
{
    private bool isParaLensesOn = false;

    public GameObject enterPkWorldButton;
    public GameObject ImageTargetCharSitting;
    public GameObject ImageTargetPKAudio;

    public void ShowEnterPkWorldButton()
    {
        isParaLensesOn = !isParaLensesOn;


        enterPkWorldButton.SetActive(isParaLensesOn);
        ImageTargetCharSitting.SetActive(isParaLensesOn);
        ImageTargetPKAudio.SetActive(isParaLensesOn);
    }

    public bool getIsParaLensesOn()
    {
        return isParaLensesOn;
    }
    
}
