using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehavior : MonoBehaviour
{
    #region Singleton
    public static UIBehavior instance;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one reference of UIBehavior!");
            return;
        }

        instance = this;
    }
    #endregion
    
    public enum UiState{
        RoamState,
        BattleState
    }

    UiState state;
    [SerializeField] GameObject roamUi;
    [SerializeField] GameObject battleUi;
 
    void Start()
    {
        state = UiState.RoamState;
        SetState(state);
    }

    public void SetState(UiState newState)
    {
        state = newState;
        if (state == UiState.RoamState)
        {
            roamUi.SetActive(true);
            battleUi.SetActive(false);
        }
        else if (state == UiState.BattleState)
        {
            roamUi.SetActive(false);
            battleUi.SetActive(true);
        }
    }

    public UiState GetState()
    {
        return state;
    }
}
