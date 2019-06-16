using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLookManager : MonoBehaviour
{
    [SerializeField] Sprite[] mask;
    [SerializeField] float rotationSpeed = 55f, growthDif = 0.5f, growthSpeed = 0.65f, startingSize = 2f;
    
    bool growing = false;
    
    float rotation;

    void Start ()
    {
        transform.localScale = new Vector3(startingSize, startingSize, startingSize);
        rotation = Random.Range(-rotationSpeed, rotationSpeed);
        GetComponent<SpriteRenderer>().sprite = mask[Random.Range(0, mask.Length)];
        var boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }
	
	void Update ()
    {
        Rotate();
        Grow();
    }

    void Grow()
    {
        float growthAmount = Time.deltaTime * growthSpeed;
        if (growing)
        {
            transform.localScale += new Vector3(growthAmount, growthAmount, growthAmount);
            if(transform.localScale.x > startingSize + growthDif)
            {
                growing = false;
            }
        }
        else
        {
            transform.localScale -= new Vector3(growthAmount, growthAmount, growthAmount);
            if (transform.localScale.x < startingSize - growthDif)
            {
                growing = true;
            }
        }
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * rotation));
    }
}
