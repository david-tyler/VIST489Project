using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexBehavior : MonoBehaviour
{
    #region Singleton
    public static PokedexBehavior instance;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of PokedexBehavior!");
            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField] Animator optionsPanelAnimator;
    [SerializeField] Button optionButton;
    [SerializeField] GameObject pokedex;
    [SerializeField] GameObject focusedDisplay;

    [SerializeField] RectTransform pokdexTransform;
    [SerializeField] RectTransform focusedDisplayTransform;
    private Vector2 onScreenPositionPokedex;
    private Vector2 offScreenPositionPokedex;
    public float duration = 0.5f; // Adjust as needed

    [SerializeField] CanvasGroup focusedDisplayCanvasGroup;
    public float fadeDuration = 0.5f;


    public enum PokedexState
    {
        HomeMenu,
        PokemonFocused
    }

    PokedexState state;

    void Start()
    {
        onScreenPositionPokedex = pokdexTransform.anchoredPosition;

        float width = pokdexTransform.rect.width;
        offScreenPositionPokedex = new Vector2(onScreenPositionPokedex.x - (width + 20), onScreenPositionPokedex.y);

        pokdexTransform.anchoredPosition = offScreenPositionPokedex;
    }

    public void SlideOut()
    {
        StartCoroutine(MoveUI(offScreenPositionPokedex));
    }

    public void SlideIn()
    {
        StartCoroutine(MoveUI(onScreenPositionPokedex));
    }

    IEnumerator MoveUI(Vector2 targetPosition)
    {

        float elapsed = 0f;
        Vector2 startingPosition = pokdexTransform.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            pokdexTransform.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }
        pokdexTransform.anchoredPosition = targetPosition;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeFocusedDisplay(0, 1));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeFocusedDisplay(1, 0));
    }

    IEnumerator FadeFocusedDisplay(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        focusedDisplayCanvasGroup.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            focusedDisplayCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        focusedDisplayCanvasGroup.alpha = endAlpha;

        // Optional: Disable interactivity when fully transparent
        if (endAlpha == 0)
        {
            focusedDisplayCanvasGroup.interactable = false;
            focusedDisplayCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            focusedDisplayCanvasGroup.interactable = true;
            focusedDisplayCanvasGroup.blocksRaycasts = true;
        }
    }
    public void OpenPokedex(bool status)
    {
    
        state = PokedexState.HomeMenu;
        if (status == true)
        {
            optionButton.enabled = false;
            SlideIn();
        }
        else
        {
            SlideOut();

        }
    }

    public void ReturnToState()
    {
        Debug.Log(state);
        if (state == PokedexState.HomeMenu)
        {
            // return to options
            OpenPokedex(false);
            optionsPanelAnimator.SetTrigger("Open");
            optionButton.enabled = true;
            
            
        }
        else if(state == PokedexState.PokemonFocused)
        {
            // return to home menu
            state = PokedexState.HomeMenu;
            FadeOut();
            OpenPokedex(true);
        }
        
    }

    public void OpenFocusDisplay()
    {
        
        state = PokedexState.PokemonFocused;
        SlideOut();
        FadeIn();
        
    }
}
