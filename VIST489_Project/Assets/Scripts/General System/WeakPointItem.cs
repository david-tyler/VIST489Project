using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeakPointItem : MonoBehaviour
{
    [SerializeField] string description;
    [SerializeField] int maxUses;
    public Enemy enemy;
    [SerializeField] TextMeshProUGUI text;

    private int uses;
    private string usesDescription;

    GameSystemBehavior gameSystem;
    bool shownWeakPoints;

    void Start()
    {
        uses = maxUses;
        usesDescription = "(" + uses + "/" + maxUses + ")";
        text.text = description + " " + usesDescription;
        shownWeakPoints = false;
    }

    public void UseItem()
    {
        bool used = false;


        gameSystem = GameSystemBehavior.instance;
        GameSystemBehavior.NarrativeEvent state = gameSystem.GetCurrentState();
        if (uses > 0)
        {
            switch (state)
            {
                case GameSystemBehavior.NarrativeEvent.ZoroarkBattle:
                    if (shownWeakPoints == false)
                    {
                        enemy.ShowWeakPoints();
                        shownWeakPoints = true;
                        used = true;
                    }
                    
                    break;
            }

            if (used)
            {
                uses -= 1;
                uses = Mathf.Clamp(uses, 0, maxUses);
                usesDescription = "(" + uses + "/" + maxUses + ")";
                text.text = description + " " + usesDescription;
                
            }
            
        }
       
        
    }

}
