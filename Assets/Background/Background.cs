using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] float minSpeed = 3f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] ParticleSystem stars;
    [SerializeField] Camera mainCamera;

    bool playing = true;

	void Start ()
    {
        SetupBackground();
    }

    private void SetupBackground()
    {
        transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        StartCoroutine(ChangingColors());
        mainCamera.backgroundColor = new Color(Random.Range(0.15f, 0.2f), Random.Range(0.15f, 0.2f), Random.Range(0.15f, 0.2f));
        SetRandomSpeed();
    }

    IEnumerator ChangingColors()
    {
        var starPs = stars.main;
        float combining = 0f;
        Color startingColor = GetRandomColor();
        Color endingColor = GetRandomColor();
        while (playing)
        {
            if(combining == 1f)
            {
                startingColor = endingColor;
                endingColor = GetRandomColor();
                combining = 0f;
            }
            starPs.startColor = Color.Lerp(startingColor, endingColor, combining);
            combining += 0.02f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SetRandomSpeed()
    {
        var mainParticles = stars.main;
        mainParticles.startSpeedMultiplier = Random.Range(minSpeed, maxSpeed);
        stars.Stop();
        stars.Play();
    }

    Color GetRandomColor()
    {
        switch (Random.Range(0, 7))
        {
            case (0):
                return Color.blue;
            case (1):
                return Color.cyan;
            case (2):
                return Color.gray;
            case (3):
                return Color.green;
            case (4):
                return Color.magenta;
            case (5):
                return Color.white;
            case (6):
                return Color.yellow;
        }
        return Color.black;
    }
}
