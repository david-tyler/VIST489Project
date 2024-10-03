using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectApproach : MonoBehaviour
{
    PopUpSystem pop;

    public GameObject YouNeedToFindTheDoorPopUp;
    public TMPro.TextMeshProUGUI text_YouNeedToFindTheDoorPopUp;
    public Animator YouNeedToFindTheDoorPopUpAnimator;

    void OnTriggerEnter(Collider other)
    {
        
    }
}
