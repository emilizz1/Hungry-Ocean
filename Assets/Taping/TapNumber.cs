using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapNumber : MonoBehaviour
{
    [SerializeField] int maxNumberOfTaps = 100;
    [SerializeField] GameObject fill;
    
    float numberOfTaps;

    void Start()
    {
        fill.transform.localPosition = new Vector3(-680f, 0f, 0f);
        numberOfTaps = maxNumberOfTaps;
        UpdateImage();
        
    }

    public void RemoveATap()
    {
        numberOfTaps--;
        UpdateImage();
        if (numberOfTaps <= 0)
        {
            FindObjectOfType<LostCondition>().GiveLostCondition("Out of Taps");
        }
    }

    void UpdateImage()
    {
        float fillAmount = 1 - numberOfTaps / maxNumberOfTaps;
        fill.transform.localPosition = new Vector3(Mathf.Lerp(0, 1, fillAmount) * 680f - 680f, 0f, 0f);
    }

    public void AddTaps(int amount)
    {
        numberOfTaps += amount;
        GetComponentInChildren<Text>().text = " Taps +" + amount.ToString();
        UpdateImage();
        Invoke("RemoveAddedText", 1f);
    }

    void RemoveAddedText()
    {
        GetComponentInChildren<Text>().text = " Taps";
    }

    public float GetTapProc()
    {
        return 1 - numberOfTaps / maxNumberOfTaps;
    }
}
