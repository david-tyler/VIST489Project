using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject objectPrefab;

    public float spawnRate = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnRate -= Time.deltaTime;

        if (spawnRate <= 0.0f)
        {

            float spawnPosX = RandomizeSpawnPosition();
            Instantiate(objectPrefab, new Vector3(spawnPosX, leftBound.transform.position.y, leftBound.transform.position.z), Quaternion.identity);
            spawnRate = 1.0f;

        }
    }

    public float RandomizeSpawnPosition()
    {

        float pos = Random.Range(leftBound.transform.position.x, rightBound.transform.position.x);


        return pos;
    }
}
