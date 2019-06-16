using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    const string HIGH_SCORE = "HighScore";
    const string LEVELS_FINISHED = "LevelsFinished";

    void Start()
    {
        if(FindObjectOfType<SaveLoad>().LoadInt(HIGH_SCORE) > 0)
        {
            GetComponent<Text>().text = "HIGH SCORE - " + FindObjectOfType<SaveLoad>().LoadInt(HIGH_SCORE) +
                "\n LEVEL " + FindObjectOfType<SaveLoad>().LoadInt(LEVELS_FINISHED);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
