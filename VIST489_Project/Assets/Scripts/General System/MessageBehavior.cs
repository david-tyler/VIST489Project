using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class MessageBehavior : MonoBehaviour
{
    #region Singleton
    public static MessageBehavior instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of MessageBehavior!");
            return;
        }

        instance = this;
    }
    #endregion

    private bool haveMessage = false;
    private bool displayingMessage = false;
    public GameObject MessageButton;
    private PopUpSystem popUp;
    private string MessageText = "";
    public Animator MessageButtonAnimator;
    public GameObject MessageNotification;
    

    public void DisplayMessage()
    {
        if (string.IsNullOrWhiteSpace(MessageText) == false && displayingMessage == false)
        {
            popUp = PopUpSystem.instance;
            popUp.PopUp(MessageText);
            displayingMessage = true;
        }
        if (MessageNotification.activeSelf == true)
        {
            MessageNotification.SetActive(false);
        }
        
        haveMessage = false;
    }
    public void SetDisplayingMessage()
    {
        displayingMessage = false;
    }
    public void SetHaveMessage(bool status)
    {
        haveMessage = status;
        if (haveMessage == true)
        {
            MessageNotification.SetActive(true);
            MessageButtonAnimator.SetTrigger("Shake");
            Handheld.Vibrate(); // vibrate phone whenever player gets a message
        }
        
       
        
    }
    public void SetMessageText(string text)
    {
        MessageText = text;
    }
}
