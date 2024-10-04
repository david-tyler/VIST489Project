using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaze : MonoBehaviour
{
    public List<GameObject> objectsToSpawn = new List<GameObject>();
    public Transform environmentPivot;
    public Vector3 offset;

    public void SpawnObjects()
    {
        for(int i = 0; i < objectsToSpawn.Count; i++)
        {
            objectsToSpawn[i].SetActive(true);
        }
    }

    public void MoveEnvironmentToMatch()
    {
        StartCoroutine(Fade());
    }


    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(1.0f);
        environmentPivot.position = this.transform.position;


    }
}
