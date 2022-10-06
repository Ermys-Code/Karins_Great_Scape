using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    Rigidbody control;
    public bool gettingDamage, gettingOpDamage;
    public float gettingDamageCounter, deadCounter;
    
    public int life;
    public bool isHealer;
    public GameObject player;

    void Start()
    {
        control = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gettingDamage)
        {
            gettingDamageCounter += Time.deltaTime;
            if (gettingDamageCounter > 0.25f)
            {
                gettingDamage = false;
                gettingDamageCounter = 0;
            }
        }
        if (gettingOpDamage)
        {
            deadCounter += Time.deltaTime;
            if (deadCounter > 1f)
            {
                if(GetComponent<MushroomEnemy>() != null)
                {
                    GetComponent<MushroomEnemy>().isDying = true;
                    GetComponent<MushroomEnemy>().DeathAnim();
                }
                if (!isHealer && GetComponent<SlimeEnemy>() == null) player.GetComponent<PlayerRigidBody>().enemyCount++;
                if (isHealer) player.GetComponent<PlayerRigidBody>().life++;
                player.GetComponent<PlayerRigidBody>().AutoHeal();
                gettingOpDamage = false;
                deadCounter = 0;
                if (GetComponent<MushroomEnemy>() == null) Destroy(gameObject);
            }                               
        }
    }

    public void GetDamage(int _minusLife, Vector3 _pushDir)
    {
        if (!gettingDamage)
        {
            life -= _minusLife;
            gettingDamage = true;
            print(_minusLife);
        }
        if(life <= 0)
        {
            if (GetComponent<MushroomEnemy>() != null && GetComponent<MushroomEnemy>().isDying != true)
            {
                GetComponent<MushroomEnemy>().isDying = true;
                GetComponent<MushroomEnemy>().DeathAnim();
                player.GetComponent<PlayerRigidBody>().enemyCount++;
            }
            //if (!isHealer && GetComponent<SlimeEnemy>() == null) player.GetComponent<PlayerRigidBody>().enemyCount++;
            if (isHealer)
            {
                player.GetComponent<PlayerRigidBody>().life++;
                player.GetComponent<PlayerRigidBody>().UpdateHud();
            }
            if (GetComponent<SlimeEnemy>() != null || GetComponent<FrogEnemy>() != null) Destroy(gameObject);
        }
        control.velocity = new Vector3(0, 0, 0);
        if (GetComponent<SlimeEnemy>() != null)
        {
            player.GetComponent<PlayerRigidBody>().slimeCount++;
            player.GetComponent<PlayerRigidBody>().UpdateHud();
            StaticPlayerValues.PlayerValues.slimeCount++;
        }
        if(GetComponent<GoatEnemy>() != null)
        {
            if (GetComponent<GoatEnemy>().isJumping == true)
            {
                GetComponent<GoatEnemy>().isJumping = false;
                
            }
            control.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
            control.AddForce(_pushDir * 1f, ForceMode.Impulse);
        }
        if (GetComponent<FrogEnemy>() != null)
        {
            if (GetComponent<FrogEnemy>().isJumping == true)
            {
                GetComponent<FrogEnemy>().isJumping = false;

            }
            control.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
            control.AddForce(_pushDir * 1f, ForceMode.Impulse);
        }
        if (GetComponent<MushroomEnemy>() != null)
        {
            if (GetComponent<MushroomEnemy>().isJumping == true)
            {
                GetComponent<MushroomEnemy>().isJumping = false;

            }
            control.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
            control.AddForce(_pushDir * 1f, ForceMode.Impulse);
        }
    }

    public void GetOpDamage(Vector3 _pushDir)
    {
        control.velocity = new Vector3(0, 0, 0);
        if (GetComponent<GoatEnemy>() != null)
        {
            if (GetComponent<GoatEnemy>().isJumping == true)
            {
                GetComponent<GoatEnemy>().isJumping = false;
            }
            gettingOpDamage = true;
            control.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            control.AddForce(_pushDir * 10f, ForceMode.Impulse);
        }
        if (GetComponent<FrogEnemy>() != null)
        {
            print("lo hago");
            if (GetComponent<FrogEnemy>().isJumping == true)
            {
                GetComponent<FrogEnemy>().isJumping = false;
            }
            gettingOpDamage = true;
            control.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            control.AddForce(_pushDir * 10f, ForceMode.Impulse);
        }
        if (GetComponent<MushroomEnemy>() != null)
        {
            if (GetComponent<MushroomEnemy>().isJumping == true)
            {
                GetComponent<MushroomEnemy>().isJumping = false;
            }
            gettingOpDamage = true;
            control.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            control.AddForce(_pushDir * 10f, ForceMode.Impulse);
        }
    }
}
