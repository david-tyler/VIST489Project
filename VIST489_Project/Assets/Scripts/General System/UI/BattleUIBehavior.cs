using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIBehavior : MonoBehaviour
{
    [SerializeField] GameObject initialMenu;
    [SerializeField] GameObject fightMenu;


    public void OpenFightMenu()
    {
        initialMenu.SetActive(false);
        fightMenu.SetActive(true);
    }

    public void OpenInitalState()
    {
        initialMenu.SetActive(true);
        fightMenu.SetActive(false);
    }



}
