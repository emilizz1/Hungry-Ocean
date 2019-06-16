using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    [SerializeField] int damageToLose = 100;
    [SerializeField] bool tutorial = false;
    [SerializeField] GameObject fill;

    float maxDamageToLose;

    void Start()
    {
        if (!tutorial)
        {
            fill.transform.localPosition = new Vector3(-680f, 0f, 0f);
        }
        maxDamageToLose = damageToLose;
    }

    void Update()
    {
        CheckIfPossibleToFinish();
    }

    void CheckIfPossibleToFinish()
    {
        UpdateImage();
        if (damageToLose <= 0)
        {
            FindObjectOfType<LostCondition>().GiveLostCondition("Too much damage taken");
        }
    }
   
    void UpdateImage()
    {
        if (!tutorial)
        {
            float fillAmount = 1 - damageToLose / maxDamageToLose;
            fill.transform.localPosition = new Vector3(Mathf.Lerp(0, 1, fillAmount) * 680f - 680f, 0f, 0f);
        }
    }

    public void DamageDealt(int damage)
    {
        damageToLose -= damage;
        GetComponentInChildren<Text>().text = " Damage +" + damage.ToString();
        Invoke("RemoveAddedText", 1f);
    }

    void RemoveAddedText()
    {
        GetComponentInChildren<Text>().text = " Damage";
    }

    public float GetDamageProc()
    {
        return 1 - damageToLose / maxDamageToLose;
    }
}
