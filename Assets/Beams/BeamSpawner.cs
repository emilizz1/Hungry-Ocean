using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSpawner : MonoBehaviour
{
    [SerializeField] Vector2[] removedPositions;
    [SerializeField] int objectsToSpawn = 4;
    [SerializeField] GameObject simpleBeam;
    [SerializeField] GameObject crossBeam;
    [SerializeField] GameObject circleBeam;

    int gridType;
    List< Vector3> usedPositions = new List<Vector3>();
    List< Vector3> movedPositions = new List<Vector3>();

    List<Vector2> grid = new List<Vector2>();

    void Start()
    {
        gridType = Random.Range(0, 2);
        AddGrid();
        for (int i = 0; i < objectsToSpawn; i++)
        {
            SpawnBeam(Random.Range(0, 3));
        }
    }

    void AddGrid()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int o = 0; o < 9; o++)
            {
                grid.Add(new Vector2(i, o));
            }
        }
        foreach(Vector2 pos in removedPositions)
        {
            grid.Remove(pos);
        }
    }

    void SpawnBeam(int beamToSpawn)
    {
        Vector3 spawnLocation = GetSpawnLocation();
        switch (beamToSpawn)
        {
            case (0):
                Instantiate(circleBeam, spawnLocation, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), transform);
                break;
            case (1):
                Instantiate(crossBeam, spawnLocation, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), transform);
                break;
            case (2):
                var spawnedBeam = Instantiate(simpleBeam, spawnLocation, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), transform);
                spawnedBeam.GetComponent<BeamLine>().GiveTarget(GetMovedPosition(spawnedBeam.transform.position));
                break;
        }
    }

    Vector2 GetMovedPosition(Vector2 movingFrom)
    {
        switch(Random.Range(0, 4))
        {
            case (0):
                if (movingFrom.x + 9.2f < 20f)
                {
                    movingFrom.x += 9.2f;
                    movedPositions.Add(movingFrom);
                }
                break;
            case (1):
                if (movingFrom.x - 9.2f > -20f)
                {
                    movingFrom.x -= 9.2f;
                    movedPositions.Add(movingFrom);
                }
                break;
            case (2):
                if (movingFrom.y - 7.6f > -30f)
                {
                    movingFrom.y -= 7.6f;
                    movedPositions.Add(movingFrom);
                }
                break;
            case (3):
                if (movingFrom.y + 7.6f > 30f)
                {
                    movingFrom.y += 7.6f;
                    movedPositions.Add(movingFrom);
                }
                break;
        }
        return movingFrom;
    }

    Vector3 GetSpawnLocation()
    {
        while (true)
        {
            int gridSpot = Random.Range(0, grid.Count);
            if ((grid[gridSpot].x + grid[gridSpot].y) % 2 == gridType)
            {
                Vector3 newLocation = new Vector3();
                newLocation.x = (grid[gridSpot].x - 2) * 9.2f;
                newLocation.y = (grid[gridSpot].y - 4) * 7.6f;
                newLocation.z = 0f;
                grid.Remove(new Vector2(grid[gridSpot].x, grid[gridSpot].y));
                return newLocation;
            }
        }
    }
}
