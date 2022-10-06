using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public Transform doorOpen, checkPoint;
    public GameObject player;

    bool itsOpen;

    void Start()
    {

    }

    void Update()
    {
        if (itsOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpen.position, 10 * Time.deltaTime);
            print("cambio");
            player.GetComponent<PlayerRigidBody>().checkPoint = checkPoint;
            player.GetComponent<PlayerRigidBody>().openingDoor = false;
        }
        if(transform.position == doorOpen.position)
        {
            itsOpen = false;
        }
    }

    public void OpenDoor()
    {
        itsOpen = true;
    }
}
