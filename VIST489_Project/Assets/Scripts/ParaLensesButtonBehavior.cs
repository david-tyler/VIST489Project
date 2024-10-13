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
    private List<Collider> platforms = new List<Collider>();
    
    public GameObject ImageTargetEnterPkWorld;
    public GameObject ImageTargetPKAudio;
    public TMPro.TextMeshProUGUI ParaLensesText;



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
        isParaLensesOn = !isParaLensesOn;
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
