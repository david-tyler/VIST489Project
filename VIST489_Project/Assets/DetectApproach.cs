using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectApproach : MonoBehaviour
{
    PopUpSystem popUp;

    public GameObject YouNeedToFindTheDoorPopUp;
    public TMPro.TextMeshProUGUI text_YouNeedToFindTheDoorPopUp;
    public Animator YouNeedToFindTheDoorPopUpAnimator;

    void OnTriggerEnter(Collider other)
    {
        popUp = PopUpSystem.instance;
        popUp.popUpBox = YouNeedToFindTheDoorPopUp;
        popUp.popUpText = text_YouNeedToFindTheDoorPopUp;
        popUp.animator = YouNeedToFindTheDoorPopUpAnimator;
        popUp.PopUp(text_YouNeedToFindTheDoorPopUp.text);
    }
}
