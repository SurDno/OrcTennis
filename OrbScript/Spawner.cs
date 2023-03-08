using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   public GameObject objectToSpawn;
    public float spawnDelay = 1f;
    public float spawnRadius = 5f;
    public float spawnY = 0f; // Set the desired Y-axis value

    void Start () {
        InvokeRepeating("SpawnObject", spawnDelay, spawnDelay);
    }

    void SpawnObject () {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = spawnY; // Set the Y-axis value
        Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
    }
}
