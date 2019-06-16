using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    
    void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed);
    }
}
