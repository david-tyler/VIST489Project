using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GameSystemBehavior : MonoBehaviour
{
    public GameObject pkWorldBttn;
    private ParaLensesButtonBehavior paraLenses;

    // Start is called before the first frame update
    void Start()
    {
        paraLenses = this.GetComponent<ParaLensesButtonBehavior>();

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    
}
