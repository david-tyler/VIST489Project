using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectApproach : MonoBehaviour
{
    PopUpSystem popUp;

    public bool fired = false;
    public GameObject YouNeedToFindTheDoorPopUp;
    public TMPro.TextMeshProUGUI text_YouNeedToFindTheDoorPopUp;
    public Animator YouNeedToFindTheDoorPopUpAnimator;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided");
        if(!fired)
        {
            popUp = PopUpSystem.instance;
            popUp.popUpBox = YouNeedToFindTheDoorPopUp;
            popUp.popUpText = text_YouNeedToFindTheDoorPopUp;
            popUp.animator = YouNeedToFindTheDoorPopUpAnimator;
            popUp.PopUp(text_YouNeedToFindTheDoorPopUp.text);
        }

        fired = true;
    }
}
