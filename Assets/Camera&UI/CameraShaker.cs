using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0f;
    [SerializeField] float shakeAmount = 0.7f;
    [SerializeField] float decreaseFactor = 1f;

    Vector3 originalPos;
    
	void Start ()
    {
        originalPos = transform.localPosition;
	}
	
	void Update ()
    {
		if(shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
        }
	}

    public void AddShakeDuration(float amount)
    {
        shakeDuration += amount;
    }
}
