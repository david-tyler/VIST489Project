using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallTower : MonoBehaviour
{

    public GlyphPuzzleController controller;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSelfToCurrentList()
    {
        controller.currentOrder.Add(this.gameObject);
    }
}
