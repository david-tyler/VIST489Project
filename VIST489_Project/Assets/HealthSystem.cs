using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public List<GameObject> sprites = new List<GameObject>();
    public int health = 3;
    public int crystalCount = 3;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i< sprites.Count; i++)
        {
            if(i < health)
            {
                sprites[i].SetActive(true);
            }
            else
            {
                sprites[i].SetActive(false);
            }
        }
    }
}
