using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIOptionButtonsBehavior : MonoBehaviour
{

    [SerializeField] Animator optionPanelAnimator;
    [SerializeField] Animator inventoryAnimator;
    [SerializeField] Button optionButton;

    private bool toggleOptionButton = false;
    private bool enableInventory;
    private bool enableSettings;

    void Start()
    {
        enableInventory = false;
        enableSettings = false;
    }

    PokedexBehavior pokedexScript;

    public void ToggleOptionButtons()
    {
        toggleOptionButton = !toggleOptionButton;

        if (toggleOptionButton == true)
        {
            optionPanelAnimator.SetTrigger("Open");

        }
        else
        {
            optionPanelAnimator.SetTrigger("Close");

        }

    }

    public void TogglePokedex()
    {
        pokedexScript = PokedexBehavior.instance;
        
        pokedexScript.OpenPokedex(true);
        optionPanelAnimator.SetTrigger("Close");
    }

    public void ToggleInventory()
    {
        enableInventory = !enableInventory;

        if (enableInventory == true)
        {
            inventoryAnimator.SetTrigger("Open");
            optionPanelAnimator.SetTrigger("Close");
            optionButton.enabled = false;
        }
        else
        {
            inventoryAnimator.SetTrigger("Close");
            optionPanelAnimator.SetTrigger("Open");
            optionButton.enabled = true;
        }
            
    }
    public void ToggleSetting()
    {
        enableSettings = !enableSettings;
    }
}
