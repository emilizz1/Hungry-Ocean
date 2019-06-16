using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public int currentLevel = 1;

	void Start ()
    {
        DontDestroyOnLoad(gameObject);
        CheckCurrentLevel();
        if (FindObjectsOfType<LevelHolder>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void CheckCurrentLevel()
    {
        int highiestLevel = 0;
        foreach (LevelHolder level in FindObjectsOfType<LevelHolder>())
        {
            if (highiestLevel < level.currentLevel)
            {
                highiestLevel = level.currentLevel;
            }
        }
        foreach (LevelHolder level in FindObjectsOfType<LevelHolder>())
        {
            level.currentLevel = highiestLevel;
        }
    }
}
