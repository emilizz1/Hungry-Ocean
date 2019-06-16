using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comet : MonoBehaviour
{
    [SerializeField] float flightSpeedMax;
    [SerializeField] float flightSpeed;
    [SerializeField] GameObject explosionOnHit;
    [SerializeField] float loopingTimer = 0.5f;
    [SerializeField] int damage = 1;
    [SerializeField] float damageDelay = 2f;
    [SerializeField] ParticleSystem bigGlow;
    [SerializeField] AudioClip[] cometStoppedSound;
    [SerializeField] AudioClip[] cometHitSound;
    [Range(0f, 1f)] [SerializeField] float soundVolume = 0.5f;

    bool puzzle = false;
    bool itPassed = false;

    Rigidbody2D rb;
    Quaternion startRotation;
    float lastTimeLooped = 0f;
    float timeCreated;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        timeCreated = Time.time;
        if (!puzzle)
        {
            var mainParticles = bigGlow.main;
            mainParticles.startColor = new Color(0.5f, 0f, 1f);
        }
    }

    void Update()
    {
        if(bigGlow != null && IsItDestructable())
        {
            var mainParticles = bigGlow.main;
            mainParticles.startColor = Color.red;
            bigGlow = null;
        }
        rb.AddForce(transform.up * Time.deltaTime * flightSpeed, ForceMode2D.Impulse);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -flightSpeedMax, flightSpeedMax), Mathf.Clamp(rb.velocity.y, -flightSpeedMax, flightSpeedMax));
        transform.rotation = startRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<UpgradeController>() && IsItDestructable()) // destroys itself when colliding with everything except Upgrade
        {
            if (collision.gameObject.GetComponent<BlackHole>())
            {
                DisplayDamageNumber(true, collision);
                AudioSource.PlayClipAtPoint(cometHitSound[Random.Range(0, cometHitSound.Length)], Camera.main.transform.position, soundVolume);
            }
            else if (collision.gameObject.GetComponent<Star>())
            {
                DisplayDamageNumber(false, collision);
                AudioSource.PlayClipAtPoint(cometHitSound[Random.Range(0, cometHitSound.Length)], Camera.main.transform.position, soundVolume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(cometStoppedSound[Random.Range(0, cometStoppedSound.Length)], Camera.main.transform.position, soundVolume);
            }
            DestroyComet(false);
        }
    }

    public void DestroyComet(bool tap)
    {
        if(tap)
        {
            FindObjectOfType<ScoreCounter>().AddPoints(5 * FindObjectOfType<LevelHolder>().currentLevel);
            AudioSource.PlayClipAtPoint(cometStoppedSound[Random.Range(0, cometStoppedSound.Length)], Camera.main.transform.position, soundVolume);
        }
        Instantiate(explosionOnHit, transform.position, Quaternion.identity, gameObject.transform.parent);
        Destroy(gameObject); 
    }

    void DisplayDamageNumber(bool fullDamage, Collision2D collision)
    {
        BlackholeDamageNumber damageNumber = FindObjectOfType<BlackholeDamageNumber>();
        GameObject numberInstance = Instantiate(damageNumber.GetNumber(), collision.GetContact(0).point, Quaternion.identity, damageNumber.transform);
        if (fullDamage)
        {
            numberInstance.GetComponent<Text>().text = "+" + damage.ToString();
        }
        else
        {
            numberInstance.GetComponent<Text>().text = "-" + (damage/2).ToString();
        }
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

    public bool DidItPass()
    {
        if (itPassed)
        {
            return false;
        }
        else
        {
            itPassed = true;
            return true;
        }
    }

    public void GiveStartingRotation(Quaternion startingRot)
    {
        startRotation = startingRot;
    }

    public int GetDamageDone()
    {
        return damage;
    }

    public bool IsItDestructable()
    {
        if (!puzzle)
        {
            return Time.time - damageDelay >= timeCreated;
        }
        else
        {
            return true;
        }
    }

    public void PlayingPuzzleLevel()
    {
        puzzle = true;
    }
}
