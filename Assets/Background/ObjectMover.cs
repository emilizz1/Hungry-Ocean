using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] Vector2 firstPos;
    [SerializeField] Vector2 secondPos;
    [SerializeField] float speed = 0.1f;

    Vector2 currentTarget;

	void Start ()
    {
        if (Random.Range(0, 2) == 1)
        {
            currentTarget = secondPos;
        }
        else
        {
            currentTarget = firstPos;
        }
	}
	
	void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed);
        if(transform.position.x == firstPos.x && transform.position.y == firstPos.y)
        {
            currentTarget = secondPos;
        }
        else if(transform.position.x == secondPos.x && transform.position.y == secondPos.y)
        {
            currentTarget = firstPos;
        }
	}
}
