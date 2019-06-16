using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpawner : MonoBehaviour
{
    [SerializeField] BlackHole portalPrefab;
    [SerializeField] Vector2[] spawnPositions;
    [SerializeField] int numberOfBHToSpawn = 2;
    [SerializeField] float timeBetweenSpawns = 10f;

    bool spawningFinished = false;

    void Start()
    {
        StartCoroutine(SpawnPortal());
    }

    IEnumerator SpawnPortal()
    {
        BlackHole myPortal = portalPrefab;
        while (numberOfBHToSpawn > 0)
        {
            yield return new WaitForSecondsRealtime(timeBetweenSpawns);
            Instantiate(myPortal, spawnPositions[Random.Range(0, spawnPositions.Length)], Quaternion.identity, transform);
            numberOfBHToSpawn--;
        }
        spawningFinished = true;
    }

    public bool GetSpawningFinished()
    {
        return spawningFinished;
    }
}
