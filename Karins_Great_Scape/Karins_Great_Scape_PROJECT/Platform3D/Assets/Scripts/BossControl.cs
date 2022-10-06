using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossControl : MonoBehaviour
{
    public int coresLeft = 3;
    public GameObject barrier, stairs, limits, proyectil, weakSpot, bossStart;
    public bool bossBattle;
    public Transform player, startPoint, shotPoint;
    public LayerMask playerMask;
    public Animator anim;
    public Material bossMat;

    Rigidbody control;
    float speed = 3, sprintSpeed = 7.5f, life = 20;
    Vector3 moveDir, endSprint;
    bool attacking, called, gettingDamage;
    public float nextAttack, attackTimer, nextAttackTime, sprintTimer, shootTimer = 0.5f, shootCounter, getDamageTimer, punchTimer;
    float ragueTimer;
    bool rague;

    void Start()
    {
        control = GetComponent<Rigidbody>();
        nextAttack = Random.Range(0, 3);
        nextAttackTime = Random.Range(5, 15);
        bossMat.SetTexture("_BaseMap", Resources.Load<Texture>("KingText/Base0"));
        bossMat.SetTexture("_BumpMap", Resources.Load<Texture>("KingText/Normal0"));
    }

    void Update()
    {
        if (coresLeft <= 0)
        {
            Destroy(barrier.gameObject);
            stairs.SetActive(true);
        }
        if (bossBattle)
        {
            if(player.GetComponent<PlayerRigidBody>().bossBattle == false) player.GetComponent<PlayerRigidBody>().bossBattle = true;
            if (limits.gameObject.activeSelf == false) limits.gameObject.SetActive(true);

            if (!rague) ragueTimer += Time.deltaTime;
            if (ragueTimer >= 180)
            {
                rague = true;
                ragueTimer = 0;
                player.GetComponent<PlayerRigidBody>().bossRague = true;
            }
            if (!attacking)
            {
                Movement();
                attackTimer += Time.deltaTime;
                if (attackTimer >= nextAttackTime)
                {
                    attackTimer = 0;
                    attacking = true;
                    if (nextAttack == 0)
                    {
                        anim.SetBool("RunAttack", true);
                    }
                    else if (nextAttack == 1)
                    {
                        anim.SetBool("FireAttack", true);
                    }
                    else if (nextAttack == 2)
                    {
                        print("DEBERIA FUNCIONAR (:");
                        anim.SetBool("PunchAttack", true);
                        print(anim.GetBool("PunchAttack") + " dbeawurofawhebfouahb");
                    }
                }
            }
            
            if (attacking)
            {
                if (nextAttack == 0 && !called)
                {
                    print("sprint");
                    called = true;
                    SprintAttack();
                }
                else if (nextAttack == 0 && called)
                {
                    SprintMovement();
                }
                else if (nextAttack == 1 && !called)
                {
                    bossMat.SetTexture("_BaseMap", Resources.Load<Texture>("KingText/Base1"));
                    bossMat.SetTexture("_BumpMap", Resources.Load<Texture>("KingText/Normal1"));
                    FireAttack();
                }
                else if (nextAttack == 2 && !called)
                {
                    PunchAttack();
                }
            }

            if (gettingDamage) getDamageTimer += Time.deltaTime;
            if(getDamageTimer >= 1)
            {
                gettingDamage = false;
                getDamageTimer = 0;
            }
        }

    }

    void Movement()
    {
        Vector3 finalPos = player.position;
        finalPos.y = transform.position.y;
        transform.LookAt(finalPos);

        moveDir = new Vector3(0, moveDir.y, speed);
        moveDir = transform.TransformDirection(moveDir);
        control.MovePosition(control.position + moveDir * Time.deltaTime);
    }

    void SprintAttack()
    {        
        Vector3 playerPosition = player.position;
        playerPosition.y = transform.position.y;
        transform.LookAt(playerPosition);
        called = true;
        //sobra
    }

    void SprintMovement()
    {

        if(sprintTimer < 2.5f)
        {
            sprintTimer += Time.deltaTime;
            moveDir = new Vector3(0, moveDir.y, sprintSpeed);
            moveDir = transform.TransformDirection(moveDir);
            control.MovePosition(control.position + moveDir * Time.deltaTime);
        }
        else
        {
            nextAttack = 4;
            anim.SetBool("RunAttack", false);
            anim.SetBool("Tired", true);
            bossMat.SetTexture("_BaseMap", Resources.Load<Texture>("KingText/Base2"));
            bossMat.SetTexture("_BumpMap", Resources.Load<Texture>("KingText/Normal2"));
            StartCoroutine(VulnerabilityTime());
        }
    }

    void FireAttack()
    {
        if(shootCounter < 3)
        {
            shootTimer -= Time.deltaTime;
            if(shootTimer <= 0f)
            {
                GameObject newProyectil = Instantiate(proyectil, shotPoint.position, Quaternion.identity);
                newProyectil.transform.parent = gameObject.transform;
                shootCounter++;
                shootTimer = 0.5f;
            }
        }
        else
        {
            called = true;
            anim.SetBool("FireAttack", false);
            anim.SetBool("Tired", true);
            bossMat.SetTexture("_BaseMap", Resources.Load<Texture>("KingText/Base2"));
            bossMat.SetTexture("_BumpMap", Resources.Load<Texture>("KingText/Normal2"));
            StartCoroutine(VulnerabilityTime());
        }
    }

    void PunchAttack()
    {
        if(punchTimer <= 1f)
        {
            punchTimer += Time.deltaTime;
            Vector3 hitboxSize = new Vector3(2, 4, 2);
            Vector3 hitboxPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f);
            Collider[] allObjects = Physics.OverlapBox(transform.position, hitboxSize, Quaternion.identity, playerMask);

            for (int i = allObjects.Length - 1; i >= 0; i--)
            {
                if (allObjects[i].GetComponent<PlayerRigidBody>() != null)
                {
                    print("aataque boss");
                    allObjects[i].GetComponent<PlayerRigidBody>().GetBossDamage(transform.forward);          
                }
            }
        }
        else
        {
            called = true;
            anim.SetBool("PunchAttack", false);
            anim.SetBool("Tired", true);
            bossMat.SetTexture("_BaseMap", Resources.Load<Texture>("KingText/Base2"));
            bossMat.SetTexture("_BumpMap", Resources.Load<Texture>("KingText/Normal2"));
            punchTimer = 0;
            StartCoroutine(VulnerabilityTime());
        }
        
    }

    IEnumerator VulnerabilityTime()
    {
        if (weakSpot.activeSelf == false) weakSpot.SetActive(true);
        yield return new WaitForSeconds(5f);
        sprintTimer = 0;
        shootCounter = 0;
        shootTimer = 0.5f;
        CalculateNextAttack();
    }
    
    void CalculateNextAttack()
    {
        nextAttack = Random.Range(0, 3);
        nextAttackTime = Random.Range(5, 15);
        if (weakSpot.activeSelf == true) weakSpot.SetActive(false);
        attacking = false;
        called = false;
        anim.SetBool("Tired", false);
        bossMat.SetTexture("_BaseMap", Resources.Load<Texture>("KingText/Base0"));
        bossMat.SetTexture("_BumpMap", Resources.Load<Texture>("KingText/Normal0"));
    }

    public void GetDamage(int _minusLife)
    {
        if (!gettingDamage)
        {
            print("Boss AU");
            life -= _minusLife;
            gettingDamage = true;
            player.GetComponent<PlayerRigidBody>().bossLifeImage.fillAmount = life / 20f;
            if (life <= 0)
            {
                if (player.GetComponent<PlayerRigidBody>().finalBattleTimer <= 60f) StaticPlayerValues.PlayerValues.killBoss = true;
                SceneManager.LoadScene(3);
            }
        }
    }

    public void Restart()
    {
        transform.position = startPoint.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        life = 20;
        attacking = false;
        called = false;
        gettingDamage = false;
        nextAttack = Random.Range(0, 3);
        nextAttackTime = Random.Range(5, 15);
        attackTimer = 0;
        sprintTimer = 0;
        shootTimer = 0;
        shootCounter = 0;
        getDamageTimer = 0;
        bossBattle = false;
        limits.gameObject.SetActive(false);
        bossStart.SetActive(true);
        rague = false;
        ragueTimer = 0;
        player.GetComponent<PlayerRigidBody>().bossRague = false;
        player.GetComponent<PlayerRigidBody>().bossBattle = false;
        player.GetComponent<PlayerRigidBody>().finalBattleTimer = 0;
        player.GetComponent<PlayerRigidBody>().bossLifeImage.fillAmount = 1;
        anim.SetBool("PunchAttack", false);
        anim.SetBool("RunAttack", false);
        anim.SetBool("FireAttack", false);
        anim.SetBool("Tired", false);
        bossMat.SetTexture("_BaseMap", Resources.Load<Texture>("KingText/Base0"));
        bossMat.SetTexture("_BumpMap", Resources.Load<Texture>("KingText/Normal0"));
    }

    void OnDrawGizmos()
    {
        Vector3 hitboxSize = new Vector3(2, 4, 2);
        Vector3 hitboxPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f);
        Gizmos.DrawWireCube(hitboxPosition, hitboxSize);
    }
}
