using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] AudioClip levelCompleted;
    [Range(0f, 1f)] [SerializeField] float soundVolume = 0.5f;
    [SerializeField] int levelToLoadOnWin;

    BlackHoleSpawner portalSpawner;
    bool nextLevel = false;

    void Start ()
    {
        portalSpawner = FindObjectOfType<BlackHoleSpawner>();
        UpdateText();
    }

    void Update()
    {
        CheckIfLevelEnded();
    }

    void CheckIfLevelEnded()
    {
        if (portalSpawner.GetSpawningFinished() && !IsBlackHoleAlive())
        {
            FindObjectOfType<EndLevelFlash>().EndLevel();
            if (!nextLevel)
            {
                PrepareForNextLevel();
            }
        }
    }

    bool IsBlackHoleAlive()
    {
        foreach(BlackHole blackHole in FindObjectsOfType<BlackHole>())
        {
            if (blackHole.IsAlive())
            {
                return true;
            }
        }
        return false;
    }

    private void PrepareForNextLevel()
    {
        FindObjectOfType<Timer>().playing = false;
        if (FindObjectOfType<CometSpawner>())
        {
            FindObjectOfType<CometSpawner>().gameObject.SetActive(false);
        }
        FindObjectOfType<ScoreCounter>().LevelCompleted();
        AudioSource.PlayClipAtPoint(levelCompleted, Camera.main.transform.position, soundVolume);
        Invoke("startLoadingNextScene", 2f);
        FindObjectOfType<LevelHolder>().currentLevel++;
        nextLevel = true;
    }

    void startLoadingNextScene()
    {
        FindObjectOfType<LoadScene>().mLoadScene(levelToLoadOnWin);
    }

    void UpdateText()
    {
        GetComponent<Text>().text = FindObjectOfType<LevelHolder>().currentLevel.ToString();
    }
}
