using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLine : MonoBehaviour
{
    [SerializeField] GameObject[] topParts;
    [SerializeField] GameObject[] bottomParts;
    
    float rotationSpeed;
    float shrinkSpeed;
    float moveSpeed;
    CapsuleCollider2D capsuleCollider;
    Vector2 startingPos;
    Vector2 targetPos;
    Vector2 currentTarget;

    private void Start()
    {
        transform.GetChild(0).localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        moveSpeed = Random.Range(0.5f, 0.8f);
        shrinkSpeed = Random.Range(0.1f, 0.2f);
        rotationSpeed = Random.Range(-40, 40f);
        capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
        transform.Rotate(new Vector3(0f, 0f, 45 * Random.Range(0, 3)));
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        Shrink();
        if (currentTarget != null)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
        if (Mathf.Abs(transform.position.x - currentTarget.x) < 0.05f && Mathf.Abs(transform.position.y - currentTarget.y) < 0.05f)
        {
            if (currentTarget == targetPos)
            {
                currentTarget = startingPos;
            }
            else
            {
                currentTarget = targetPos;
            }
        }
    }

    private void Shrink()
    {
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, (topParts[0].transform.localPosition.y * 2) + 0.15f);
        foreach (GameObject part in bottomParts)
        {
            part.transform.localPosition = new Vector3(0f, part.transform.localPosition.y - (shrinkSpeed * Time.deltaTime), 0f);
        }
        foreach (GameObject part in topParts)
        {
            part.transform.localPosition = new Vector3(0f, part.transform.localPosition.y + (shrinkSpeed * Time.deltaTime), 0f);
        }
        if (topParts[0].transform.localPosition.y <= 0.135f || topParts[0].transform.localPosition.y >= 0.37f)
        {
            shrinkSpeed = shrinkSpeed * -1;
        }
    }

    public void GiveTarget(Vector2 target)
    {
        targetPos = target;
        startingPos = transform.position;
        currentTarget = targetPos;
    }
}
