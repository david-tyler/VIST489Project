using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphPuzzleController : MonoBehaviour
{
    public List<GameObject> correctOrder = new List<GameObject>();
    public List<GameObject> currentOrder = new List<GameObject>();
    public List<GameObject> placedBalls = new List<GameObject>();


    public void Start()
    {
       
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
            GameObject.Destroy(placedBalls[i]);
        }

        placedBalls.Clear();
    }



    
}