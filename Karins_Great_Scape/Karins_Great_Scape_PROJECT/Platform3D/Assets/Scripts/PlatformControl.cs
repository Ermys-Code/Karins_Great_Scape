using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    public Transform point1, point2;

    bool goTo2;
    float speed = 5;

    void Start()
    {
        
    }

    void Update()
    {
        if (goTo2)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, point2.position, speed * Time.deltaTime);
            if (transform.position == point2.position) goTo2 = false;
        }
        if (!goTo2)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, point1.position, speed * Time.deltaTime);
            if (transform.position == point1.position) goTo2 = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
