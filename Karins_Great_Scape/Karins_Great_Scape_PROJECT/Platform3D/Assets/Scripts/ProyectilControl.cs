using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilControl : MonoBehaviour
{
    Transform player;
    Vector3 playerPosition;
    float speed = 15f;

    void Start()
    {
        player = GetComponentInParent<BossControl>().player;
        playerPosition = player.position;
    }

    void Update()
    {
        if (transform.parent != null) transform.parent = null;
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        if(transform.position == playerPosition)
        {
            Destroy(gameObject);
        }
    }
}
