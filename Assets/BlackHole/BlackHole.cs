using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DreamStarGen.Algorithms;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] GameObject absorbingStar, clashWithComet;
    [SerializeField] bool tutorial = false;
    [SerializeField] AudioClip[] cometImpact;
    [SerializeField] AudioClip starEaten;
    [Range(0f, 1f)] [SerializeField] float soundVolume = 0.5f;

    bool alive = true;

    Vector3 pos;
    CameraShaker cameraShaker;
    LifePoints lifePoints;
    Rigidbody myRigidbody;
    Vector3 targetPos;
    float lerp;

    void Start()
    {
        pos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 60f));
        lifePoints = GetComponent<LifePoints>();
        myRigidbody = GetComponent<Rigidbody>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        if (!tutorial)
        {
            GetNewTargetPos();
            CheckRotation();
        }
    }

    void Update()
    {
        if (alive)
        {
            if (!tutorial && checkIfOutOfBounds() || !tutorial && myRigidbody.velocity.magnitude< minSpeed)
            {
                GetNewTargetPos();
                CheckRotation();
            }
            if(myRigidbody.velocity.magnitude > maxSpeed)
            {
                myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, maxSpeed);
            }
        }
    }

    private void CheckRotation()
    {
        
        Vector3 rotation = targetPos - transform.position;
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(angle + 90f, -90f, 90f));

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Star>())
        {
            StarCollided(collision);
        }
        else if (collision.gameObject.GetComponent<Comet>() && collision.gameObject.GetComponent<Comet>().IsItDestructable())
        {
            CometCollided(collision);
        }
    }

    private void StarCollided(Collision collision)
    {
        AudioSource.PlayClipAtPoint(starEaten, Camera.main.transform.position, soundVolume);
        Instantiate(absorbingStar, collision.GetContact(0).point, Quaternion.identity, collision.gameObject.transform);
        var star = collision.gameObject.GetComponent<Star>();
        StartCoroutine(AbsorbingStar(star));
        star.DestroyStar(false);
    }

    private void CometCollided(Collision collision)
    {
        AudioSource.PlayClipAtPoint(cometImpact[Random.Range(0, cometImpact.Length)], Camera.main.transform.position, soundVolume);
        Destroy(Instantiate(clashWithComet, collision.GetContact(0).point, Quaternion.identity), clashWithComet.GetComponent<ParticleSystem>().main.duration);
        GetHealed(collision);
        cameraShaker.AddShakeDuration(0.2f);
    }

    private void GetHealed(Collision collision)
    {
        int healing = collision.gameObject.GetComponent<Comet>().GetDamageDone();
        lifePoints.RemoveLife(-healing);
        FindObjectOfType<Ammo>().DamageDealt(healing);
    }

    IEnumerator AbsorbingStar(Star figure)
    {
        int absorbedBullets = figure.GetBulletAmount();
        while (figure)
        {
            figure.transform.position = Vector2.MoveTowards(figure.transform.position, transform.position, 0.5f);
            if (absorbedBullets > 0)
            {
                lifePoints.RemoveLife();
                absorbedBullets--;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void GetNewTargetPos()
    {
        float minX = pos.x * 0.3f;
        float maxX = pos.x * -0.7f;
        float minY = pos.y * 0.3f;
        float maxY = pos.y * -0.7f;
        targetPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
        myRigidbody.AddForce((targetPos - transform.position) * moveSpeed, ForceMode.Impulse);
    }

    public void BlackholeDied()
    {
        alive = false;
        //Destroy(particle);
    }

    // used for when finished to stop
    public void SetAlive(bool set)
    {
        alive = set;
    }

    bool checkIfOutOfBounds()
    {
        if (transform.position.x < pos.x * 0.3f ||
            transform.position.x > pos.x * -0.7f || 
            transform.position.y < pos.y * 0.3f || 
            transform.position.y > pos.y * -0.7f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAlive()
    {
        return alive;
    }

    public bool GetIsItTutorial()
    {
        return tutorial;
    }
}