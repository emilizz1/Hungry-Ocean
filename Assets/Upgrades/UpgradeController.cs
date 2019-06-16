using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] GameObject deathParticles;
    [SerializeField] GameObject tapedParticles;
    [SerializeField] AudioClip upgradeDestroyed;
    [SerializeField] AudioClip upgradeTaken;
    [Range(0f, 1f)] [SerializeField] float soundVolume = 0.5f;
    [SerializeField] int extraTime;
    [SerializeField] int extraDamage;
    [SerializeField] int extraTaps;

    void Start()
    {
        transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayParticlesAndSound(deathParticles, upgradeDestroyed);
        Destroy(gameObject);
    }

    public void tapped()
    {
        PlayParticlesAndSound(tapedParticles, upgradeTaken);
        GiveBonus();
        Destroy(gameObject);
    }

    void GiveBonus()
    {
        if (extraTime > 0)
        {
            FindObjectOfType<Timer>().AddTime(extraTime);
        }
        else if (extraDamage > 0)
        {
            FindObjectOfType<Ammo>().DamageDealt(-extraDamage);
        }
        else if (extraTaps > 0)
        {
            FindObjectOfType<TapNumber>().AddTaps(extraTaps);
        }
    }

    void PlayParticlesAndSound(GameObject particlesToPlay, AudioClip audioToPlay)
    {
        AudioSource.PlayClipAtPoint(audioToPlay, Camera.main.transform.position, soundVolume);
        var particles = Instantiate(particlesToPlay, transform.position, Quaternion.identity, transform.parent);
        Destroy(particles, particles.GetComponent<ParticleSystem>().main.duration);
    }
}
