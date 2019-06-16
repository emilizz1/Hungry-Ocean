using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Vector2 multipliyier;
    [SerializeField] bool ignoreComets = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Star>())
        {
            
            if (other.gameObject.GetComponent<Star>().ShouldItLoop())
            {
                LoopObject(other);
            }
        }
        else
        {
            if (other.gameObject.GetComponent<Comet>() && !ignoreComets)
            {
                if (other.gameObject.GetComponent<Comet>().ShouldItLoop() || other.gameObject.GetComponent<Comet>().DidItPass())
                {
                    LoopObject(other);
                }
            }
        }
    }

    private void LoopObject(Collider2D other)
    {
        var pos = other.gameObject.transform.position;
        pos = pos * multipliyier;
        other.gameObject.transform.position = pos;
    }
}
