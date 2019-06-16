using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlendModes;

public class EndLevelFlash : MonoBehaviour
{
    [SerializeField] float transitionSpeed = 1f;
    [SerializeField] GameObject[] stars;
    [SerializeField] GameObject center;
    [SerializeField] GameObject centerObject;
    [SerializeField] float objectMaxSize;
    [SerializeField] Sprite[] centerSprites;
    [SerializeField] BlendMode[] BlendModes;
    [SerializeField] float rotation;

    bool once = true;

	public void EndLevel()
    {
        if (once)
        {
            var myStar = stars[Random.Range(0, stars.Length)];
            myStar.SetActive(true);
            StartCoroutine(GrowObject(myStar, 1f));
            centerObject.GetComponent<Image>().sprite = centerSprites[Random.Range(0, centerSprites.Length)];
            StartCoroutine(GrowObject(centerObject, 0.1f));
            StartCoroutine(GrowObject(center, 1f));
            StartCoroutine(RotateObject(centerObject, Random.Range(-rotation, -5)));
            GetComponentInChildren<BlendModeEffect>().BlendMode = BlendModes[Random.Range(0, BlendModes.Length)];
            once = false;
        }
    }

    IEnumerator GrowObject(GameObject myObject, float sizeMultiplier)
    {
        myObject.SetActive(true);
        myObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        while(myObject.transform.localScale.x < objectMaxSize * sizeMultiplier)
        {
            myObject.transform.localScale += new Vector3((Time.deltaTime * transitionSpeed * sizeMultiplier), (Time.deltaTime * transitionSpeed * sizeMultiplier), (Time.deltaTime * transitionSpeed * sizeMultiplier));
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator RotateObject(GameObject myObject, float rotate)
    {
        while (myObject.transform.localScale.x < objectMaxSize)
        {
            myObject.transform.Rotate(new Vector3(0f, 0f, rotate));
            yield return new WaitForEndOfFrame();
        }
    }

    
}
