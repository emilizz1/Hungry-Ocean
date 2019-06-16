using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DreamStarGen.Algorithms;

public class Star : MonoBehaviour
{
    [Range(0.1f, 1f)] [SerializeField] float starRadiusIncrease = 0.3f;
    [SerializeField] float loopingTimer = 1f;
    [SerializeField] GameObject[] centerParticles;
    [SerializeField] ParticleSystem clashWithComet;
    [SerializeField] AudioClip[] cometHitStarClip;
    [SerializeField] float soundVolume;

    bool startedExploding = false;
    bool beenHit = false;

    float rotationSpeed;
    int bulletAmount = 7;
    float lastTimeLooped = 0f;
    CircleCollider2D myCollider;
    GameObject myCentralParticles;

    void Start()
    {
        rotationSpeed = Random.Range(-40, 40f);
        myCollider = GetComponent<CircleCollider2D>();
        myCentralParticles = Instantiate(centerParticles[Random.Range(0, centerParticles.Length)], transform.position, Quaternion.identity, transform);
        AddSizeToCenterParticles(bulletAmount);
    }

    void Update()
    {
        if (!beenHit && gameObject != null)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            UpdateFigureLifes();
        }
        else if (bulletAmount <= 0 && !startedExploding)
        {
            DestroyStar(true);
        }
        if (!startedExploding && gameObject != null)
        {
            UpdateFigureLifes();
        }
    }

    public int GetBulletAmount()
    {
        return bulletAmount;
    }

    void RemoveAmmo()
    {
        beenHit = true;
        AddSizeToCenterParticles(-1);
        bulletAmount--;
    }

    public void GiveBulletsAmount(int bullets)
    {
        bulletAmount = bullets;
    }

    public bool ShouldItLoop()
    {
        if (Time.time - lastTimeLooped >= loopingTimer)
        {
            lastTimeLooped = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyStar(bool quick)
    {
        startedExploding = true;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        var myPsEmission = myCentralParticles.GetComponent<ParticleSystem>().emission;
        myPsEmission.enabled = false;
        Destroy(myCollider);
        if (quick)
        {
            StartCoroutine(ShrinkingStar(0.1f));
        }
        else
        {
            StartCoroutine(ShrinkingStar(0.01f));
        }
    }

    IEnumerator ShrinkingStar(float shrinkingSpeed)
    {
        while (startedExploding)
        {
            transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(0.01f, 0.01f), shrinkingSpeed);
            if(transform.localScale.x < 0.05f || transform.localScale.y < 0.05f)
            {
                Destroy(gameObject);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void UpdateFigureLifes()
    {
        if (!startedExploding)
        {
            float size = (bulletAmount * starRadiusIncrease) + starRadiusIncrease;
            transform.localScale = new Vector3(size, size, size);
        }
    }

    void AddSizeToCenterParticles(int times)
    {
        if (gameObject != null)
        {
            foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
            {
                var particles = particle.main;
                particles.startSizeMultiplier = particles.startSize.constantMax * 0.1f * times;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Comet>() && collision.gameObject.GetComponent<Comet>().IsItDestructable())
        {
            Destroy(Instantiate(clashWithComet, collision.GetContact(0).point, Quaternion.identity, transform), clashWithComet.main.duration);
            int damageDealtByComet = collision.gameObject.GetComponent<Comet>().GetDamageDone() / 2;//deals only half damage to stars
            StartCoroutine(RemoveStarLife(damageDealtByComet));
            if (FindObjectOfType<Ammo>())
            {
                FindObjectOfType<Ammo>().DamageDealt(damageDealtByComet);
            }
        }
    }

    public IEnumerator RemoveStarLife(int starLivesToRemove, bool shouldPlaySound = false)
    {
        if (shouldPlaySound)
        {
            AudioSource.PlayClipAtPoint(cometHitStarClip[Random.Range(0, cometHitStarClip.Length)], Camera.main.transform.position, soundVolume);
        }
        for (int i = 0; i < starLivesToRemove; i++)
        {
            RemoveAmmo();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
