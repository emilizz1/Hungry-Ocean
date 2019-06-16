using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCompleted : MonoBehaviour
{
    bool isCompleted = false;

    const string TUTORIAL_COMPLETIONS = "TutorialCompletions";

    void Awake()
    {
        var numOfBackgroundThemes = FindObjectsOfType<TutorialCompleted>().Length;
        if (numOfBackgroundThemes > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //if( FindObjectOfType<SaveLoad>().LoadInt(TUTORIAL_COMPLETIONS) >= 2)
        //{
        //    isCompleted = true;
        //}
{}
    }

    public bool GetIsTutorialCompleted()
    {
        return isCompleted;
    }

    public void TutorialFinished()
    {
        isCompleted = true;
        FindObjectOfType<SaveLoad>().SaveInt(TUTORIAL_COMPLETIONS, FindObjectOfType<SaveLoad>().LoadInt(TUTORIAL_COMPLETIONS) + 1);
    }
}
