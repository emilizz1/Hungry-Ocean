using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    bool levelCompleted = false;

    int currentScene;

    void Awake()
    {
        var numOfLoadScene = FindObjectsOfType<LoadScene>().Length;
        if (numOfLoadScene > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if(currentScene != SceneManager.GetActiveScene().buildIndex || levelCompleted)
        {
            StartCoroutine(GetComponent<StartEndLevelCanvas>().CanvasDisapearring());
            currentScene = SceneManager.GetActiveScene().buildIndex;
            levelCompleted = false;
        }
    }

    public void mLoadScene(int scene)
    {
        //preparing to load a scene
        StartCoroutine(GetComponent<StartEndLevelCanvas>().CanvasApearring(scene));
    }

    public void mLoadSameScene()
    {
        //preparing to load a scene
        StartCoroutine(FindObjectOfType<StartEndLevelCanvas>().CanvasApearring(SceneManager.GetActiveScene().buildIndex));
    }

    public void LevelCompleted()
    {
        levelCompleted = true;
    }
}
