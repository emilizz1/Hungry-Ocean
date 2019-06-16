using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamStarGen.Algorithms;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] stars;
    [SerializeField] Material[] materials;
    [SerializeField] [Range(-100, 100)] float minThrowForce_x =0f, maxThrowForce_x = 0f, minThrowForce_y = 0f, maxThrowForce_y = 0f;
    [SerializeField] float minSpawnTime = 0.8f, maxSpawnTime = 1.6f, minStartingPosX, maxStartingPosX, minStartingPosY, maxStartingPosY;
    [SerializeField] int minBulletAmount = 3, maxBulletAmount = 12, starMaxCap = 7;

    bool playing = true;

	void Start ()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (playing)
        {
            int bulletAmount = Random.Range(minBulletAmount, maxBulletAmount);
            if (FindObjectsOfType<Star>().Length <= starMaxCap)
            {
                GameObject myObject = Instantiate(stars[Random.Range(0, stars.Length)]) as GameObject;
                myObject.GetComponentInChildren<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
                myObject.GetComponentInChildren<DreamStarGen.DreamStarGenerator>()._GenerateStar();
                myObject.transform.parent = transform;
                myObject.transform.position = new Vector2(Random.Range(minStartingPosX, maxStartingPosX), Random.Range(minStartingPosY, maxStartingPosY));
                myObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(minThrowForce_x, maxThrowForce_x), Random.Range(minThrowForce_y, maxThrowForce_y)), ForceMode2D.Impulse);
                myObject.GetComponent<Star>().GiveBulletsAmount(bulletAmount);
            }
            yield return new WaitForSecondsRealtime(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
