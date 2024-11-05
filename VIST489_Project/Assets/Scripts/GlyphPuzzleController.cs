using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphPuzzleController : MonoBehaviour
{
    public List<GameObject> correctOrder = new List<GameObject>();
    public List<GameObject> currentOrder = new List<GameObject>();
    public List<GameObject> placedBalls = new List<GameObject>();

    public MeshRenderer coloredPokeball;
    public Material baseColor;

    public GameObject gate;
    public bool gateOpen = false;

    public void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (!gateOpen)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    IncorrectOrder();
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (!gateOpen)
            {
                IncorrectOrder();
            }
        }
    }

    public bool CheckOrder()
    {
        for(int i = 0; i < correctOrder.Count; i++)
        {
            if(currentOrder[i] != correctOrder[i])
            {
                IncorrectOrder();
                return false;
            }
        }

        OpenGate();
        return true;
    }

    public void IncorrectOrder()
    {
        ClearPokeBalls();
        currentOrder.Clear();
    }

    public void ClearPokeBalls()
    {
        for(int i = 0; i< placedBalls.Count; i++)
        {
            placedBalls[i].SetActive(false);
        }

        coloredPokeball.material = baseColor;
    }

    public void OpenGate()
    {
        gate.SetActive(false);
    }


    



}
