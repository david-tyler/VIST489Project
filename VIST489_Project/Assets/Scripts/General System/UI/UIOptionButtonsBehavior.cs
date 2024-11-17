using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIOptionButtonsBehavior : MonoBehaviour
{

    [SerializeField] Animator optionPanelAnimator;
    [SerializeField] Button optionButton;
    [SerializeField] RectTransform inventoryTransform;
    [SerializeField] float duration = 0.5f; // Adjust as needed
    [SerializeField] RectTransform inventoryCloseButtonTransform;

    private Vector2 onScreenPositionInventory;
    private Vector2 offScreenPositionInventory;
    
    private bool toggleOptionButton = false;
    private bool enableInventory;
    private bool enableSettings;

    void Start()
    {
        enableInventory = false;
        enableSettings = false;

        onScreenPositionInventory = inventoryTransform.anchoredPosition;

        float width = inventoryTransform.rect.width;
        offScreenPositionInventory = new Vector2(onScreenPositionInventory.x + width + inventoryCloseButtonTransform.rect.width, onScreenPositionInventory.y);

        inventoryTransform.anchoredPosition = offScreenPositionInventory;
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

    
    public void SlideOut()
    {
        enableInventory = true;
        optionPanelAnimator.SetTrigger("Open");
        StartCoroutine(MoveUI(offScreenPositionInventory,inventoryTransform));
        optionButton.interactable = true;
    }

    public void SlideIn()
    {
        enableInventory = false;
        optionPanelAnimator.SetTrigger("Close");
        optionButton.interactable = false;
        StartCoroutine(MoveUI(onScreenPositionInventory, inventoryTransform));
        
    }

    IEnumerator MoveUI(Vector2 targetPosition, RectTransform UIObject)
    {

        float elapsed = 0f;
        Vector2 startingPosition = UIObject.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            UIObject.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }
        UIObject.anchoredPosition = targetPosition;
    }
    public void ToggleSetting()
    {
        enableSettings = !enableSettings;
    }
}
