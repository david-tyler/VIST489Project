using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaLensesButtonBehavior : MonoBehaviour
{

    #region Singleton
    public static ParaLensesButtonBehavior instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of GameSystemBehavior!");
            return;
        }

        instance = this;
    }
    #endregion
    private bool isParaLensesOn = false;
    
    public GameObject ImageTargetEnterPkWorld;
    public GameObject ImageTargetPKAudio;
    public TMPro.TextMeshProUGUI ParaLensesText;

    public GameSystemBehavior gameSystem;


    private string text_EnableParaLenses = "Enable Paranormal Lenses";
    private string text_DisableParaLenses = "Disable Paranormal Lenses";

    public List<GameObject> ActiveGameObjects = new List<GameObject>();

    public void Start()
    {
        isParaLensesOn = false;
    }

    public void ToggleParanormalLenses()
    {
        // Functionality to toggle the paralenses on/off
        gameSystem = GameSystemBehavior.instance;

        isParaLensesOn = !isParaLensesOn;

        gameSystem.SetNarrativeEvent(GameSystemBehavior.NarrativeEvent.ParalensesOn, isParaLensesOn); // potentially return and see if we can avoid using isParaLensesOn

        if (isParaLensesOn == true)
        {
            ParaLensesText.text = text_DisableParaLenses;
            foreach (GameObject currentGameObject in ActiveGameObjects)
            {
                currentGameObject.SetActive(true);
            }
        }
        else if (isParaLensesOn == false)
        {
            ParaLensesText.text = text_EnableParaLenses;
            foreach (GameObject currentGameObject in ActiveGameObjects)
            {
                currentGameObject.SetActive(false);
            }
        }

        
        ImageTargetPKAudio.SetActive(isParaLensesOn);
    }

    public bool getIsParaLensesOn()
    {
        return isParaLensesOn;
    }
    
}
