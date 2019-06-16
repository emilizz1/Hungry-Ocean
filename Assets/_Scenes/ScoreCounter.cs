using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    int score = 0;
    int levelsCompleted = 0;

    void Awake()
    {
        var numOfScoreCounters = FindObjectsOfType<ScoreCounter>().Length;
        if (numOfScoreCounters > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddPoints(int amount)
    {
        score += amount;
    }

    public int GetLevelsCompleted()
    {
        return levelsCompleted;
    }

    public void LevelCompleted()
    {
        levelsCompleted++;
        AddPoints(10 * FindObjectOfType<LevelHolder>().currentLevel);
    }
}
