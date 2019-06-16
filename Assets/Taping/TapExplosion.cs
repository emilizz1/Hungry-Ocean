using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapExplosion : MonoBehaviour
{
    [SerializeField] float explosionForce = 10f;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] GameObject[] explosions;
    [SerializeField] bool tutorial = false;
    [SerializeField] AudioClip tapSFX;
    [SerializeField] AudioClip[] tapCometSFX;
    [Range(0f, 1f)] [SerializeField] float soundVolume = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(tapSFX, Camera.main.transform.position, soundVolume);
            var touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 60f));
            transform.position = touchPos;
            Explode();
        }
    }

    void Explode()
    {
        GameObject explosion = Instantiate(explosions[Random.Range(0, explosions.Length)], transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        if (!tutorial)
        {
            FindObjectOfType<TapNumber>().RemoveATap();
        }
        else
        {
            FindObjectOfType<TutorialGuide>().Tapped();
        }
        CheckForStarsInRange();
        CheckForCometsInRange();
        CheckForUpgradesInRange();
    }

    void CheckForStarsInRange()
    {
        foreach (Star figure in GetStarsInRange())
        {
            Rigidbody2D rb = figure.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce((rb.transform.position - transform.position) * explosionForce, ForceMode2D.Impulse);
            }
        }
    }

    List<Star> GetStarsInRange()
    {
        List<Star> figures = new List<Star>();
        foreach(Star figure in FindObjectsOfType<Star>())
        {
            if (Vector2.Distance(figure.transform.position, transform.position) <= explosionRadius)
            {
                figures.Add(figure);
            }            
        }
        return figures;
    }

    void CheckForCometsInRange()
    {
        foreach (Comet comet in GetCometsInRange())
        {
            Rigidbody2D rb = comet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                AudioSource.PlayClipAtPoint(tapCometSFX[Random.Range(0, tapCometSFX.Length)], Camera.main.transform.position, soundVolume);
                comet.DestroyComet(true);
            }
        }
    }

    List<Comet> GetCometsInRange()
    {
        List<Comet> comets = new List<Comet>();
        foreach (Comet comet in FindObjectsOfType<Comet>())
        {
            if (Vector2.Distance(comet.transform.position, transform.position) <= explosionRadius)
            {
                comets.Add(comet);
            }
        }
        return comets;
    }

    void CheckForUpgradesInRange()
    {
        foreach (UpgradeController upgrade in GetUpgradesInRange())
        {
            upgrade.tapped();
        }
    }

    List<UpgradeController> GetUpgradesInRange()
    {
        List<UpgradeController> upgrades = new List<UpgradeController>();
        foreach(UpgradeController upgrade in FindObjectsOfType<UpgradeController>())
        {
            if(Vector2.Distance(upgrade.transform.position, transform.position) <= explosionRadius)
            {
                upgrades.Add(upgrade);
            }
        }
        return upgrades;
    }
}
