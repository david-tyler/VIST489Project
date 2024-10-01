using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TextMeshProUGUI popUpText;

    #region Singleton
    public static PopUpSystem instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of PopUpSystem!");
            return;
        }

        instance = this;
    }
    #endregion


    void Start()
    {
        // This is the very first pop up box we see introducting the game
        PopUp(popUpText.text);
        
    }

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
