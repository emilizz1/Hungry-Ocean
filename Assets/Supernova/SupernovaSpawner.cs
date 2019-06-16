using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupernovaSpawner : MonoBehaviour
{
    [SerializeField] GameObject supernova;
    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;
    [SerializeField] float maxSupernovas = 2;
    [SerializeField] Vector2 randomSpawnLocationA;
    [SerializeField] Vector2 randomSpawnLocationB;

    bool playing = true;

    Vector3 pos;

    void Start ()
    {
        pos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 60f));
        StartCoroutine(SpawnSupernova());
    }

    IEnumerator SpawnSupernova()
    {
        while (playing)
        {
            yield return new WaitForSecondsRealtime(Random.Range(minSpawnTime, maxSpawnTime));
            if (FindObjectsOfType<Supernova>().Length < maxSupernovas)
            {
                if (randomSpawnLocationA == Vector2.zero)
                {
                    Instantiate(supernova, GetNewTargetPos(), Quaternion.identity, transform);
                }
                else
                {
                    var spawnLocation = new Vector2(Random.Range(randomSpawnLocationA.x, randomSpawnLocationB.x), Random.Range(randomSpawnLocationA.y, randomSpawnLocationB.y));
                    Instantiate(supernova, spawnLocation, Quaternion.identity, transform);
                }
            }
        }
    }

    Vector3 GetNewTargetPos()
    {
        Vector2 spawningPos = GetNewPos();
        bool found = false;
        while (found)
        {
            if (CheckForNearbyBlackHoles(spawningPos))
            {
                spawningPos = GetNewPos();
            }
            else
            {
                found = true;
            }
        }
        return spawningPos;
    }

    Vector2 GetNewPos()
    {
        float minX = pos.x * 0.3f;
        float maxX = pos.x * -0.7f;
        float minY = pos.y * 0.3f;
        float maxY = pos.y * -0.7f;
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    bool CheckForNearbyBlackHoles(Vector2 spawningPos)
    {
        bool isNearby = false;
        foreach (BlackHole blackhole in FindObjectsOfType<BlackHole>())
        {
            if(Vector2.Distance(spawningPos, blackhole.transform.position) < supernova.GetComponent<Supernova>().GetMaxRadius())
            {
                return isNearby = true;
            }
        }
        return isNearby;
    }
}
