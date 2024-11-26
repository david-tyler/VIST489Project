using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBossFight : MonoBehaviour
{
    public List<GameObject> bossFightobjects = new List<GameObject>();
    public GameObject PopUpBox; // Our dialogue box
    public Animator PopUpBoxAnimator;
    MessageBehavior messageBehavior;
    PopUpSystem popUp;

    public TMPro.TextMeshProUGUI PopUpBoxText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0;i< bossFightobjects.Count;i++)
        {
            bossFightobjects[i].SetActive(true);
        }

        popUp = PopUpSystem.instance;
        popUp.popUpBox = PopUpBox;
        popUp.popUpText = PopUpBoxText;
        popUp.animator = PopUpBoxAnimator;

        messageBehavior = MessageBehavior.instance;

        messageBehavior.SetMessageText("OH NO! A trap! Grab that remote, it's not what it appears to be! Break the crystals!");
        popUp.PopUp("OH NO! A trap! Grab that remote, it's not what it appears to be! Break the crystals!");
        messageBehavior.SetHaveMessage(true);
        
    }
}

