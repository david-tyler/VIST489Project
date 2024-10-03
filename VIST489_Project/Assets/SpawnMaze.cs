using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaze : MonoBehaviour
{
    public List<GameObject> objectsToSpawn = new List<GameObject>();

    public void SpawnObjects()
    {
        for(int i = 0; i < objectsToSpawn.Count; i++)
        {
            objectsToSpawn[i].SetActive(true);
        }
    }
}
