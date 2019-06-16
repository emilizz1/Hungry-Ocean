using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCross : MonoBehaviour
{
    [SerializeField] Transform firstBeam, secondBeam;

    float rotationSpeed;
    float firstMovingSpeed;
    float secondMovingSpeed;
    Vector3 firstTarget = new Vector3(3.25f, 0f, 0f);
    Vector3 secondTarget = new Vector3(0f, -3.25f, 0f);


    private void Start()
    {
        SetRandomRotation();
        firstMovingSpeed = Random.Range(0.2f, 0.5f);
        secondMovingSpeed = Random.Range(0.2f, 0.5f);
    }

    void SetRandomRotation()
    {
        rotationSpeed = Random.Range(-50, 50f);
        if (Mathf.Abs(rotationSpeed) < 20f)
        {
            SetRandomRotation();
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        firstBeam.localPosition = Vector2.MoveTowards(firstBeam.localPosition, firstTarget, firstMovingSpeed * Time.deltaTime);
        if(firstBeam.transform.localPosition.x >= 3.25f || firstBeam.transform.localPosition.x <= -3.25f)
        {
            firstTarget = firstTarget * -1;
        }
        secondBeam.localPosition = Vector2.MoveTowards(secondBeam.localPosition, secondTarget, secondMovingSpeed * Time.deltaTime);
        if(secondBeam.transform.localPosition.y >= 3.25f || secondBeam.transform.localPosition.y <= -3.25f)
        {
            secondTarget = secondTarget * -1;
        }
    }
}
