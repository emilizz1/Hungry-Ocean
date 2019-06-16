using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSpawner : MonoBehaviour
{
    [SerializeField] GameObject timeUpgrade, tapUpgrade, damageUpgrade;
    [SerializeField] float minSpawnTime = 2f, maxSpawnTime = 3.5f, spawnX = 26f, spawnY = 40f, force = 0.25f;

    float spawningMinValue = .5f, spawningMaxValue = .8f, currentSpawningTime = 0, currentlyLowestStat = 0;
    List<GameObject> activeUpgrades = new List<GameObject>();
    bool playing = false, hasTimeUpgrade = false, hasTapUpgrade = false, hasDamageUpgrade = false;

    void Update()
    {
        CheckTimeUpgrade();
        CheckTapUpgrade();
        CheckDamageUpgrade();
        if (playing == false && currentlyLowestStat >= spawningMinValue)
        {
            playing = true;
            StartCoroutine(SpawnUpgrades()); 
        }
    }

    IEnumerator SpawnUpgrades()
    {
        while (playing)
        {
            yield return new WaitForSeconds(currentSpawningTime);
            if (activeUpgrades.Count > 0)
            {
                var myUpgrade = Instantiate(activeUpgrades[Random.Range(0, activeUpgrades.Count)], SpawnPos(Random.Range(0, 2)), Quaternion.identity, transform);
                myUpgrade.GetComponent<Rigidbody2D>().AddForce((Vector3.zero - myUpgrade.transform.position) * force, ForceMode2D.Impulse);
            }
        }
    }

    Vector3 SpawnPos(int num)
    {
        if (num == 0)
        {
            return SpawnPointA(Random.Range(0, 2));
        }
        else
        {
            return SpawnPointB(Random.Range(0, 2));
        }
    }
    
    Vector3 SpawnPointA(int num)
    {
        if(num == 0)
        {
            return new Vector3(-spawnX, Random.Range(-spawnY, spawnY), 0f);
        }
        else
        {
            return new Vector3(spawnX, Random.Range(-spawnY, spawnY), 0f);
        }
    }

    Vector3 SpawnPointB(int num)
    {
        if(num == 0)
        {
            return new Vector3(Random.Range(-spawnX, spawnX), -spawnY, 0f);
        }
        else
        {
            return new Vector3(Random.Range(-spawnX, spawnX), spawnY, 0f);
        }
    }

    void CheckTimeUpgrade()
    {
        float timeFillAmount = FindObjectOfType<Timer>().GetTimeProc();
        CheckLowestStat(timeFillAmount);
        if (timeFillAmount > spawningMinValue)
        {
            if (!hasTimeUpgrade)
            {
                activeUpgrades.Add(timeUpgrade);
                hasTimeUpgrade = true;
            }
        }
        else
        {
            if (hasTimeUpgrade)
            {
                activeUpgrades.Remove(timeUpgrade);
                hasTimeUpgrade = false;
            }
        }
    }

    void CheckTapUpgrade()
    {
        float tapFillAmount = FindObjectOfType<TapNumber>().GetTapProc();
        CheckLowestStat(tapFillAmount);
        if (tapFillAmount > spawningMinValue)
        {
            if (!hasTapUpgrade)
            {
                activeUpgrades.Add(tapUpgrade);
                hasTapUpgrade = true;
            }
        }
        else
        {
            if (hasTapUpgrade)
            {
                activeUpgrades.Remove(tapUpgrade);
                hasTapUpgrade = false;
            }
        }
    }

    void CheckDamageUpgrade()
    {
        float damageFillAmount = FindObjectOfType<Ammo>().GetDamageProc();
        CheckLowestStat(damageFillAmount);
        if (damageFillAmount > spawningMinValue)
        {
            if (!hasDamageUpgrade)
            {
                activeUpgrades.Add(damageUpgrade);
                hasDamageUpgrade = true;
            }
        }
        else
        {
            if (hasDamageUpgrade)
            {
                activeUpgrades.Remove(damageUpgrade);
                hasDamageUpgrade = false;
            }
        }
    }

    void CheckLowestStat(float stat)
    {
        if(currentlyLowestStat < stat)
        {
            float spawningProc = ((stat - spawningMinValue) * 100) / (spawningMaxValue - spawningMinValue);
            float spawnRateValue = ((maxSpawnTime - minSpawnTime) * spawningProc) / 100;
            currentSpawningTime = spawnRateValue + minSpawnTime;
            currentlyLowestStat = stat;
        }
    }
}
