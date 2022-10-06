using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerRigidBody : MonoBehaviour
{


    Rigidbody control;
    Vector3 moveDir, pushDir;
    float speed = 6, jumpSpeed = 10, spinTimer = 5f, frontTimer = 0.5f, gettingDamageTimer, healTimer;
    float mouseX, mouseY, mouseSpeed = 3;
    float keyCount;
    float turnSmoothVelocity, turnSmoothTime = 0.1f;
    bool invertMouse = true;
    public bool isGrounded, spinActive, frontActive, gettingDamage, isJumping, opMode, openingDoor, isDying;
    public int jumpCount;
    GameObject door;
    Transform camTransform;
    bool firstKeyDialogDone;
    float attack1Fill, attack2Fill;

    public Transform checkPoint;
    public LayerMask groundMask, enemyMask, slimeMask, perfectCol;
    public GameObject frontAttackEmpty, boss, iris;
    public Transform cameraPivot;
    public Canvas canvas;
    public int life = 3;
    public bool playerPaused;
    public bool bossRague, bossBattle;
    public int enemyCount;
    public float finalBattleTimer;
    public GameObject findCore1, findCore2;
    public float totalDeaths;
    public int coinCount, slimeCount;
    public Animator anim;
    public ParticleSystem deadParticles;
    public Material karinMat;
    public Image crown1, crown2, crown3, life1, life2, life3, bossLifeImage, attack1Image, attack2Image, doorImage;
    public Text coinsText, bossTimer;
    public GameObject bossLifePanel, attacksPanel, coinsPanel, lifePanel, doorPanel;
    public GameObject lightningFloorPlane;
    public GameObject lightningParticle1, lightningParticle2, lightningParticle3;
    public AudioSource attackSound;

    void Start()
    {
        StaticPlayerValues.PlayerValues.totalDeaths = 0;
        StaticPlayerValues.PlayerValues.coinsCount = 0;
        StaticPlayerValues.PlayerValues.slimeCount = 0;
        coinCount = 0;
        slimeCount = 0;
        camTransform = Camera.main.transform;
        control = GetComponent<Rigidbody>();
        karinMat.SetTexture("_BaseMap", Resources.Load<Texture>("KarinText/0"));
    }

    void FixedUpdate()
    {
        if (playerPaused) return;
        else
        {
            if (!isDying)
            {
                if (!gettingDamage) Movement();
                fallAtack();
                DetectEnemy();
            }

        }
    }

    private void Update()
    {

        Input.GetJoystickNames();
        if (playerPaused) return;
        else
        {
            if (!isDying)
            {
                if (!gettingDamage) Jump();
                spinAttack();
                FrontAttack();
                if (opMode) AutoHeal();
                if (life > 3) life = 3;
                if (openingDoor) OpenDoor();
                if (bossBattle) BossTimerCount();
            }
            RotateRay();
        }
        BossLifeBar();
    }

    void RotateRay()
    {
        Vector3 lookPoint = camTransform.position;
        lookPoint.y = lightningParticle1.transform.position.y;
        lightningParticle1.transform.LookAt(lookPoint);
        lightningParticle2.transform.LookAt(lookPoint);
        lightningParticle3.transform.LookAt(lookPoint);

    }

    void Movement()
    {
        print(Physics.Raycast(transform.position, transform.forward, 1.2f, perfectCol));


        Vector3 spherePos = transform.position;
        spherePos.y -= 0.6f;
        isGrounded = Physics.OverlapSphere(spherePos, 0.2f, groundMask).Length > 0;
        anim.SetBool("Grounded", isGrounded);

        if (isGrounded)
        {
            jumpCount = 0;
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0F, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {
                float targertAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targertAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targertAngle, 0f) * Vector3.forward;
                if (Physics.Raycast(transform.position, transform.forward, 1.2f, perfectCol) == false) control.MovePosition(control.position + moveDir.normalized * speed * Time.deltaTime);


            }

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                anim.SetBool("Run", true);
                iris.GetComponent<FairyControl>().walking = true;
                iris.GetComponent<FairyControl>().Walk();
            }
            else
            {
                anim.SetBool("Run", false);
                iris.GetComponent<FairyControl>().walking = false;
                iris.GetComponent<FairyControl>().Walk();
            } 
                
        }
        else
        {
            anim.SetBool("Jump", control.velocity.y > 0);

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0F, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {
                float targertAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targertAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targertAngle, 0f) * Vector3.forward;

                if (Physics.Raycast(transform.position, transform.forward, 1.2f, perfectCol) == false) control.MovePosition(control.position + moveDir.normalized * speed * Time.deltaTime);
            }
        }


    }

    void Jump()
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
            {
                jumpCount++;
                isJumping = true;
                control.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && isJumping && jumpCount < 1)
            {
                print("doble salto");
                jumpCount++;
                Vector3 newVelocity = new Vector3(control.velocity.x, 0, control.velocity.z);
                control.velocity = newVelocity;
                control.AddForce(Vector3.up * (jumpSpeed - 2f), ForceMode.Impulse);
            }
        }
    }

    void spinAttack()
    {
        if (spinActive)
        {
            spinTimer += Time.deltaTime;
            Collider[] allObjects = Physics.OverlapSphere(transform.position, 1.5f, enemyMask);
            for (int i = allObjects.Length - 1; i >= 0; i--)
            {
                if (allObjects[i].GetComponent<EnemyDamage>() != null)
                {
                    allObjects[i].GetComponent<EnemyDamage>().GetDamage(2, transform.forward);
                }
                else if (allObjects[i].tag == "WeakSpot")
                {
                    boss.GetComponent<BossControl>().GetDamage(2);
                }
                else if (allObjects[i].tag == "NormalEnemy")
                {
                    allObjects[i].GetComponent<NormalEnemy>().GetDamage();
                }
            }
            if (spinTimer >= 2f)
            {
                lightningParticle1.gameObject.SetActive(false);
                lightningParticle2.gameObject.SetActive(false);
                lightningParticle3.gameObject.SetActive(false);
                lightningFloorPlane.GetComponent<LightningFloor>().ResetScale();
                lightningFloorPlane.SetActive(false);
                spinActive = false;
                spinTimer = 0;
            }
        }
        else
        {
            spinTimer += Time.deltaTime;
            attack2Fill += Time.deltaTime;
            attack2Image.fillAmount = attack2Fill / 5f;
            if (spinTimer > 5f)
            {
                spinTimer = 5f;
                attack2Fill = 5f;
            }
            if (spinTimer >= 5f && Input.GetMouseButtonDown(1) || spinTimer >= 5f && Input.GetButtonDown("Fire2"))
            {
                print("spin");
                lightningParticle1.gameObject.SetActive(true);
                lightningParticle2.gameObject.SetActive(true);
                lightningParticle3.gameObject.SetActive(true);
                lightningFloorPlane.SetActive(true);
                spinActive = true;
                anim.SetTrigger("AreaAttack");
                spinTimer = 0;
                attack2Fill = 0;
                attack2Image.fillAmount = 0;
                attackSound.clip = Resources.Load<AudioClip>("Sounds/Karin/raySound");
                attackSound.Play();
            }
        }
    }

    void FrontAttack()
    {
        if (!isGrounded) return;
        if (frontActive)
        {
            Vector3 frontHitboxSize = new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 1.6f, transform.localScale.z * 1.9f);
            frontTimer += Time.deltaTime;
            Collider[] allObjects = Physics.OverlapBox(frontAttackEmpty.transform.position, frontHitboxSize, Quaternion.identity, enemyMask);
            if (opMode)
            {
                for (int i = allObjects.Length - 1; i >= 0; i--)
                {
                    if (allObjects[i].GetComponent<EnemyDamage>() != null)
                    {
                        allObjects[i].GetComponent<EnemyDamage>().GetOpDamage(transform.forward);
                    }
                    else if (allObjects[i].tag == "Core")
                    {
                        print("llego");
                        boss.GetComponent<BossControl>().coresLeft--;
                        if (boss.GetComponent<BossControl>().coresLeft == 2) canvas.GetComponent<CanvasControl>().DestroyFirstCore();
                        if (boss.GetComponent<BossControl>().coresLeft == 0) canvas.GetComponent<CanvasControl>().DestroyLastCore();
                        Destroy(allObjects[i].gameObject);
                    }
                    else if (allObjects[i].tag == "WeakSpot")
                    {
                        print(allObjects[i].tag);
                        boss.GetComponent<BossControl>().GetDamage(1);
                    }
                    else if (allObjects[i].tag == "NormalEnemy")
                    {
                        allObjects[i].GetComponent<NormalEnemy>().GetDamage();
                    }
                }
                if (frontTimer >= 0.25f)
                {
                    frontActive = false;
                    frontTimer = 0;
                }
            }
            else
            {
                for (int i = allObjects.Length - 1; i >= 0; i--)
                {
                    if (allObjects[i].GetComponent<EnemyDamage>() != null && allObjects[i].gameObject.tag != "Shield")
                    {
                        print("has hecho daño");
                        allObjects[i].GetComponent<EnemyDamage>().GetDamage(1, transform.forward);
                    }
                    else if (allObjects[i].gameObject.tag == "ShieldHitPoint")
                    {
                        print("escudo");
                        allObjects[i].GetComponentInParent<EnemyDamage>().GetDamage(1, transform.forward);
                    }
                    else if (allObjects[i].tag == "Core")
                    {
                        print("llego");
                        boss.GetComponent<BossControl>().coresLeft--;
                        if (boss.GetComponent<BossControl>().coresLeft == 2) canvas.GetComponent<CanvasControl>().DestroyFirstCore();
                        if (boss.GetComponent<BossControl>().coresLeft == 0) canvas.GetComponent<CanvasControl>().DestroyLastCore();
                        Destroy(allObjects[i].gameObject);
                    }
                    else if (allObjects[i].tag == "WeakSpot")
                    {
                        print(allObjects[i].tag);
                        boss.GetComponent<BossControl>().GetDamage(1);
                    }
                    else if(allObjects[i].tag == "NormalEnemy")
                    {
                        allObjects[i].GetComponent<NormalEnemy>().GetDamage();
                    }
                }
                if (frontTimer >= 0.25f)
                {
                    frontActive = false;
                    frontTimer = 0;
                }
            }
        }
        else
        {
            frontTimer += Time.deltaTime;
            attack1Fill += Time.deltaTime;
            attack1Image.fillAmount = attack1Fill / 0.5f;
            if (frontTimer > 0.5f)
            {
                frontTimer = 0.5f;
                attack1Fill = 0.5f;
            }
            if (frontTimer >= 0.5f && Input.GetMouseButtonDown(0) || frontTimer >= 0.5f && Input.GetButtonDown("Fire1"))
            {
                print("front");
                frontActive = true;
                anim.SetTrigger("FrontAttack");
                frontTimer = 0;
                attack1Fill = 0;
                attack1Image.fillAmount = 0;
                attackSound.clip = Resources.Load<AudioClip>("Sounds/Karin/swordSound");
                attackSound.Play();
            }
        }
    }

    void fallAtack()
    {
        //if (isGrounded) return;
        if (control.velocity.y < 0)
        {
            Vector3 fallAttack = transform.position;
            fallAttack.y -= 0.8f;
            Collider[] allObjects = Physics.OverlapSphere(fallAttack, 0.24f, enemyMask);
            Collider[] allSlimes = Physics.OverlapSphere(fallAttack, 0.24f, slimeMask);
            //print(allObjects.Length);
            for (int i = allObjects.Length - 1; i >= 0; i--)
            {
                control.velocity = new Vector3(control.velocity.x, 0, control.velocity.z);
                control.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                print("SETAAAAA");
                if (allObjects[i].GetComponent<EnemyDamage>() != null)
                {
                    if (allObjects[i].GetComponent<GoatEnemy>() != null)
                    {
                        if (allObjects[i].GetComponent<GoatEnemy>().haveHelmet == false)
                        {
                            allObjects[i].GetComponent<EnemyDamage>().GetDamage(1, transform.forward);
                        }
                        else
                        {
                            GetDamage(0);
                        }
                    }
                    else if (allObjects[i].GetComponent<MushroomEnemy>() != null)
                    {
                        if (allObjects[i].GetComponent<MushroomEnemy>().haveHelmet == false)
                        {
                            allObjects[i].GetComponent<EnemyDamage>().GetDamage(1, transform.forward);
                        }
                        else
                        {
                            GetDamage(0);
                        }
                    }
                   
                    else
                    {
                        allObjects[i].GetComponent<EnemyDamage>().GetDamage(1, transform.forward);
                    }
                }
                else if (allObjects[i].tag == "NormalEnemy")
                {
                    allObjects[i].GetComponent<NormalEnemy>().GetDamage();
                }
            }
            for (int i = 0; i < allSlimes.Length; i++)
            {
                print("SLIMEEEE");
                control.velocity = new Vector3(control.velocity.x, 0, control.velocity.z);
                control.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                allSlimes[i].GetComponent<EnemyDamage>().GetDamage(1, transform.forward);
            }
        }
    }

    void DetectEnemy()
    {
        Vector3 point0 = transform.position;
        point0.y -= 0.6f;
        Vector3 point1 = transform.position;
        point1.y += 0.6f;
        Collider[] allObjects = Physics.OverlapCapsule(point0, point1, 0.26f, enemyMask);
        for (int i = allObjects.Length - 1; i >= 0; i--)
        {
            if (allObjects[i].tag != "Core" && allObjects[i].tag != "WeakSpot" && allObjects[i].tag != "NormalEnemy")
            {
                print(allObjects[i].tag);

                if (!gettingDamage && allObjects[i].tag != "Boss")
                {
                    pushDir = allObjects[i].transform.position - transform.position;
                    pushDir *= -1;
                    gettingDamage = true;
                    GetDamage(0);
                    if(allObjects[i].GetComponent<MushroomEnemy>() != null) allObjects[i].GetComponent<MushroomEnemy>().AttackAnim();
                }
                else if(!gettingDamage && allObjects[i].tag == "Boss")
                {
                    pushDir = allObjects[i].transform.position - transform.position;
                    pushDir *= -1;
                    gettingDamage = true;
                    GetDamage(1);
                    if (allObjects[i].GetComponent<MushroomEnemy>() != null) allObjects[i].GetComponent<MushroomEnemy>().AttackAnim();
                }
            }
        }
        if (gettingDamage)
        {
            gettingDamageTimer += Time.deltaTime;
            if (gettingDamageTimer >= 0.65f)
            {
                gettingDamage = false;
                gettingDamageTimer = 0;
            }
        }
    }

    public void AutoHeal()
    {
        healTimer += Time.deltaTime;
        if (healTimer > 10f)
        {
            life++;
            healTimer = 0;
            opMode = false;
            iris.GetComponent<FairyControl>().death = false;
            iris.GetComponent<FairyControl>().Death();
            //iris.GetComponent<FairyControl>().deathParticle.gameObject.SetActive(false);
            karinMat.SetTexture("_BaseMap", Resources.Load<Texture>("KarinText/0"));
        }
        if (life != 1)
        {
            healTimer = 0;
            opMode = false;
            iris.GetComponent<FairyControl>().death = false;
            iris.GetComponent<FairyControl>().Death();
            karinMat.SetTexture("_BaseMap", Resources.Load<Texture>("KarinText/0"));
        }
        UpdateHud();
    }

    void SetOpMode()
    {
        opMode = true;
    }

    void OpenDoor()
    {
        if (Input.GetKeyDown(KeyCode.E) && keyCount > 0 || Input.GetButtonDown("Fire3") && keyCount > 0)
        {
            keyCount--;
            door.GetComponent<DoorControl>().OpenDoor();
        }
        else if (Input.GetKeyDown(KeyCode.E) && keyCount == 0 || Input.GetButtonDown("Fire3") && keyCount == 0)
        {
            canvas.GetComponent<CanvasControl>().NoKey();
            openingDoor = false;
        }
    }

    void DeadAnimation()
    {
        isDying = true;
        ParticleSystem deadParticle = Instantiate(deadParticles, transform.GetChild(4));
        anim.SetTrigger("Death");
    }

    public void Dead()
    {
        if (bossBattle)
        {
            Respawn();
            return;
        }
        StaticPlayerValues.PlayerValues.totalDeaths++;
        transform.position = checkPoint.position;
        life = 3;
        control.velocity = new Vector3(0, 0, 0);
        karinMat.SetTexture("_BaseMap", Resources.Load<Texture>("KarinText/0"));
        isDying = false;
        UpdateHud();
    }

    void Respawn()
    {
        //animacion
        transform.position = checkPoint.position;
        life = 3;
        control.velocity = new Vector3(0, 0, 0);
        karinMat.SetTexture("_BaseMap", Resources.Load<Texture>("KarinText/0"));
        isDying = false;
        anim.Play("Idle");
        boss.GetComponent<BossControl>().Restart();
    }

    void BossTimerCount()
    {
        finalBattleTimer += Time.deltaTime;
        int tempTimer = (int)finalBattleTimer;
        bossTimer.text = tempTimer.ToString();
    }

    public void GetDamage(int _type)
    {
        if (_type == 0)
        {
            control.AddForce(pushDir * 9f, ForceMode.Impulse);
            control.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
        else if (_type == 1)
        {
            control.AddForce(Vector3.up * 15f, ForceMode.Impulse);
            control.AddForce(pushDir * 15f, ForceMode.Impulse);
        }
        if (!bossRague) life--;
        else life = 0;
        print("au");
        if (life == 1 && !opMode)
        {
            SetOpMode();
            anim.SetTrigger("IrisDeath");
            iris.GetComponent<FairyControl>().death = true;
            iris.GetComponent<FairyControl>().Death();
        }
        if (life <= 0)
        {
            print("Muerteeee");
            isDying = true;
            DeadAnimation();
        }
        UpdateHud();
    }

    public void GetBossDamage(Vector3 _pushDir)
    {
        if (!gettingDamage)
        {
            control.AddForce(Vector3.up * 7.5f, ForceMode.Impulse);
            control.AddForce(_pushDir * 18f, ForceMode.Impulse);
            if (!bossRague) life--;
            else life = 0;
            print("au");
            gettingDamage = true;
            if (life == 1 && !opMode)
            {
                SetOpMode();
                iris.GetComponent<FairyControl>().death = true;
                iris.GetComponent<FairyControl>().Death();
            }
            if (life <= 0)
            {
                print("Muerteeee");
                isDying = true;
                DeadAnimation();
            }
        }
        UpdateHud();
    }

    public void ColectCoin()
    {
        coinCount++;
        StaticPlayerValues.PlayerValues.coinsCount++;
        coinsText.text = "X " + coinCount.ToString();
    }

    public void ColectKey()
    {
        keyCount++;
        if (!firstKeyDialogDone)
        {
            canvas.GetComponent<CanvasControl>().FirstKeyDialog();
            firstKeyDialogDone = true;
        }
    }

    public void UpdateHud()
    {
        switch (slimeCount)
        {
            case 0:
                break;
            case 1:
                crown1.sprite = Resources.Load<Sprite>("Sprites/HUD/Coronita");
                break;
            case 2:
                crown2.sprite = Resources.Load<Sprite>("Sprites/HUD/Coronita");
                break;
            case 3:
                crown3.sprite = Resources.Load<Sprite>("Sprites/HUD/Coronita");
                break;
        }

        switch (life)
        {
            case 0:
                life1.sprite = Resources.Load<Sprite>("Sprites/HUD/VidaMorida");
                life2.sprite = Resources.Load<Sprite>("Sprites/HUD/VidaMorida");
                life3.sprite = Resources.Load<Sprite>("Sprites/HUD/VidaMorida");
                break;
            case 1:
                life1.sprite = Resources.Load<Sprite>("Sprites/HUD/Vida");
                life2.sprite = Resources.Load<Sprite>("Sprites/HUD/VidaMorida");
                life3.sprite = Resources.Load<Sprite>("Sprites/HUD/VidaMorida");
                break;
            case 2:
                life1.sprite = Resources.Load<Sprite>("Sprites/HUD/Vida");
                life2.sprite = Resources.Load<Sprite>("Sprites/HUD/Vida");
                life3.sprite = Resources.Load<Sprite>("Sprites/HUD/VidaMorida");
                break;
            case 3:
                life1.sprite = Resources.Load<Sprite>("Sprites/HUD/Vida");
                life2.sprite = Resources.Load<Sprite>("Sprites/HUD/Vida");
                life3.sprite = Resources.Load<Sprite>("Sprites/HUD/Vida");
                break;
        }
    }

    public void BossLifeBar()
    {
        if (bossBattle && !playerPaused) bossLifePanel.gameObject.SetActive(true);
        else bossLifePanel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            ColectCoin();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Key")
        {
            ColectKey();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Door")
        {
            door = other.gameObject;
            openingDoor = true;
            doorPanel.SetActive(true);
            if (keyCount == 0) doorImage.sprite = Resources.Load<Sprite>("Sprites/UI/OpenPanel2");
            else doorImage.sprite = Resources.Load<Sprite>("Sprites/UI/OpenPanel1");
        }

        if (other.gameObject.tag == "Trap")
        {
            DeadAnimation();
        }

        if (other.gameObject.tag == "Spawn")
        {
            checkPoint = other.transform.parent.gameObject.transform;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "FindCore")
        {
            canvas.GetComponent<CanvasControl>().FindCore();
            Destroy(findCore1.gameObject);
            Destroy(findCore2.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            doorPanel.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 spherePos = transform.position;
        spherePos.y -= 0.6f;
        Gizmos.DrawWireSphere(spherePos, 0.2f);

        Gizmos.DrawWireSphere(transform.position, 1.5f);

        Vector3 frontHitboxSize = new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 1.6f, transform.localScale.z * 1.9f);
        Gizmos.DrawWireCube(frontAttackEmpty.transform.position, frontHitboxSize);
    }
}