using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TextMeshProUGUI popUpText;

    public void PopUp(string text)
    {
        popUpBox.SetActive(true);

        popUpText.text = text;
        animator.SetTrigger("pop");
    }

    public void RemovePopUp()
    {
        popUpBox.SetActive(false);
        animator.SetTrigger("close");
    }
}
