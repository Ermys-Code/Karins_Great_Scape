using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarinDead : MonoBehaviour
{
    public Material karinMat;
    public AudioSource sound;

    PlayerRigidBody player;
    Transform deadParticle;

    void IrisDeath()
    {
        karinMat.SetTexture("_BaseMap", Resources.Load<Texture>("KarinText/1"));
    }

    void Dead()
    {
        print("animacionDone");
        player = GetComponentInParent<PlayerRigidBody>();
        deadParticle = this.gameObject.transform.Find("DeadParticleSistem(Clone)");
        player.Dead();
        Destroy(deadParticle.gameObject);
        player.isDying = false;
    }

    void Steps()
    {
        sound.clip = Resources.Load<AudioClip>("Sounds/Karin/stepsSound");
        sound.Play();
    }

    void Land()
    {
        sound.clip = Resources.Load<AudioClip>("Sounds/Karin/landSound");
        sound.Play();
    }
}
