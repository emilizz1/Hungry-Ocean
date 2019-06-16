using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePoints : MonoBehaviour
{
    [SerializeField] int minLifePoints = 10;
    [SerializeField] int maxLifePoints = 15;
    [SerializeField] float shrinkingSpeed = 1f;
    [SerializeField] ParticleSystem[] blackHoleDeath;
    [SerializeField] AudioClip blackholeCompleted;
    [Range(0f, 1f)] [SerializeField] float soundVolume = 0.5f;

    bool alive = true;
    
    int currentLife;
    GameObject lifeNumber;
    Text lifeNumberText;
    int score;

    void Start()
    {
        currentLife = Random.Range(minLifePoints, maxLifePoints);
        score = currentLife;
        lifeNumber = FindObjectOfType<BlackholeNumber>().GetNumber();
        lifeNumberText = lifeNumber.GetComponent<Text>();
    }

    void Update()
    {
            UpdateLife();
    }

    public void RemoveLife(int amount = 1)
    {
        currentLife -= amount;
        UpdateLife();
        if(amount > 0)
        {
            score += 5;
        }
        if (currentLife <= 0)
        {
            alive = false;
            Destroy(lifeNumber);
            StartCoroutine(BlackHoleDeath());
        }
    }

    public int GetCurrentLifePoints()
    {
        return currentLife;
    }

    void UpdateLife()
    {
        if (alive)
        {
            lifeNumberText.text = currentLife.ToString();
            lifeNumber.transform.position = transform.position;
        }
    }

    IEnumerator BlackHoleDeath()
    {
        float deathTime = PlayDeathSounds();
        KillBlackhole();
        CheckIfItsthelast();
        foreach (ParticleSystem particle in blackHoleDeath)
        {
            particle.Play();
        }
        while (deathTime >= 0)
        {
            deathTime -= Time.deltaTime;
            transform.localScale -= new Vector3(shrinkingSpeed * Time.deltaTime, shrinkingSpeed * Time.deltaTime, 0f);
            if(transform.localScale.x <= 0)
            {
                deathTime = -1;
            }
            yield return new WaitForEndOfFrame();
        }
        FindObjectOfType<ScoreCounter>().AddPoints(score * 2);
        Destroy(gameObject);
    }

    private float PlayDeathSounds()
    {
        var clipToPlay = blackholeCompleted;
        AudioSource.PlayClipAtPoint(clipToPlay, Camera.main.transform.position, soundVolume);
        float deathTime = clipToPlay.length;
        return deathTime;
    }

    private void KillBlackhole()
    {
        GetComponent<BlackHole>().BlackholeDied();
        Destroy(GetComponent<CircleCollider2D>());
        FindObjectOfType<ScreenClickRipple>().AddRipple(transform.position);
    }

    public bool IsItAlive()
    {
        return alive;
    }

    void CheckIfItsthelast()
    {
        if (!GetComponent<BlackHole>().GetIsItTutorial())
        {
            if (FindObjectsOfType<BlackHole>().Length <= 1 && FindObjectOfType<BlackHoleSpawner>().GetSpawningFinished())
            {
                FindObjectOfType<Timer>().StopCounting();
            }
        }
    }
}