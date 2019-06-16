using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStar : MonoBehaviour
{
    [SerializeField] GameObject[] stars;

    void Start()
    {
        stars[Random.Range(0, stars.Length)].gameObject.SetActive(true);
    }
}
