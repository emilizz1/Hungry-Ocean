using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBack : MonoBehaviour
{
    DreamStarGen.DreamStarGenerator blackholeBack;

    void Start ()
    {
		blackholeBack = GetComponent<DreamStarGen.DreamStarGenerator>();
        blackholeBack.a = Random.Range(5f, 71f);
    }

}
