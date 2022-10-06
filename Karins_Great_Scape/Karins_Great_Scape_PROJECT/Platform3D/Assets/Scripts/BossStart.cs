using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    public GameObject boss, canvas;

    Camera cam;
    GameObject player;
    AudioSource sound;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            cam = Camera.main;
            sound = cam.transform.GetComponent<AudioSource>();
            sound.clip = Resources.Load<AudioClip>("Sounds/Music/ost2");
            sound.Play();
            other.gameObject.GetComponent<PlayerRigidBody>().anim.SetBool("Run", false);
            other.gameObject.GetComponent<PlayerRigidBody>().playerPaused = true;
            canvas.GetComponent<CanvasControl>().BossEncounter();
            player = other.gameObject;
            gameObject.SetActive(false);
        }
    }

    public void StartBattle()
    {
        player.gameObject.GetComponent<PlayerRigidBody>().playerPaused = false;
        boss.GetComponent<BossControl>().bossBattle = true;
    }
}
