using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaze : MonoBehaviour
{
    public List<GameObject> objectsToSpawn = new List<GameObject>();
    public Transform environmentPivot;
    public Vector3 offset;

    GameSystemBehavior gameSystem;
    public string klefkiEndHallway = "If you go back out to the hallway, a Klefki randomly spawned there. Maybe that's a clue, try looking in your book.";
    public void SpawnObjects()
    {
        for(int i = 0; i < objectsToSpawn.Count; i++)
        {
            objectsToSpawn[i].SetActive(true);
        }

        gameSystem.SetHaveMessage(true);
        gameSystem.SetMessageText(klefkiEndHallway);
    }

    public void MoveEnvironmentToMatch()
    {
        StartCoroutine(WaitToCalibrate());
    }
    public IEnumerator WaitToCalibrate()
    {
        yield return new WaitForSeconds(1.0f);
        environmentPivot.position = this.transform.position;
    }
}
