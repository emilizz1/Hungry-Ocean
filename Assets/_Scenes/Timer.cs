using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool playing = true;

    [SerializeField] float playingTime;
    [SerializeField] GameObject fill;

    float currentTime;

    void Start()
    {
        currentTime = 0f;
        fill.transform.localPosition = new Vector3(-680f,0f,0f);
    }

    void Update()
    {
        if (playing)
        {
            currentTime += Time.deltaTime;
            float fillingNeeded = currentTime / playingTime;
            fill.transform.localPosition = new Vector3( Mathf.Lerp(0, 1, fillingNeeded) * 680f -680f, 0f, 0f);
            if (currentTime >= playingTime)
            {
                FindObjectOfType<LostCondition>().GiveLostCondition("Out of Time");
            }
        }
    }

    public void AddTime(int amount)
    {
        playingTime += amount;
        GetComponentInChildren<Text>().text = " Time +" + amount.ToString();
        Invoke("RemoveAddedText", 1f);
    }

    void RemoveAddedText()
    {
        GetComponentInChildren<Text>().text = " Time";
    }

    public float GetMaxTime()
    {
        return playingTime;
    }

    public float GetTimeProc()
    {
        return currentTime / playingTime;
    }

    public void StopCounting()
    {
        playing = false;
    }
}
