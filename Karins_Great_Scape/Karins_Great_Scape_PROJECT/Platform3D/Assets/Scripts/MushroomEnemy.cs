using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    Rigidbody control;
    public bool isGrounded, playerOnArea, playerDetect;
    RaycastHit hitWalls;
    Vector3 moveDir;
    float speed = 1.5f, jumpSpeed = 11;
    int currentPathPoint;
    float detectCounter = 2;
    bool detectCounterOn;

    public Transform player, center;
    public LayerMask groundMask, playerMask;
    public List<Transform> pathPoints;
    public bool isJumping, haveHelmet, isDying;
    public float followDistance;
    public Animator anim;

    void Start()
    {
        control = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isDying) return;
        if (!playerOnArea)
        {
            Move();
            Jump();
            DetectPlayer();
        }
        else
        {
            FollowPlayer();
            DetectPlayer();
        }
        
        WalkAnims();
    }

    void Move()
    {
        Vector3 finalPos = pathPoints[currentPathPoint].position;
        finalPos.y = transform.position.y;
        transform.LookAt(finalPos);

        moveDir = new Vector3(0, moveDir.y, speed);
        moveDir = transform.TransformDirection(moveDir);
        control.MovePosition(control.position + moveDir * Time.deltaTime);

        /*if(control.velocity != Vector3.zero) anim.SetBool("Run", true);
        else anim.SetBool("Run", false);*/


        float dist = Vector3.Distance(transform.position, finalPos);
        if (dist <= 1)
        {
            currentPathPoint++;
            if (currentPathPoint >= pathPoints.Count) currentPathPoint = 0;
        }
    }

    void Jump()
    {
        isGrounded = Physics.OverlapSphere(transform.position, 0.2f, groundMask).Length > 0;
        Vector3 rayPos = transform.position;
        rayPos.y += 0.1f;

        if (isGrounded && !isJumping && Physics.Raycast(rayPos, transform.forward, out hitWalls, 1, groundMask))
        {
            control.velocity = new Vector3(control.velocity.x, 0, control.velocity.z);
            control.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            isJumping = true;
        }

        if (isJumping && control.velocity.y < 0) isJumping = false;
    }

    void FollowPlayer()
    {
        float dist = Vector3.Distance(transform.position, center.position);
        if (dist < followDistance)
        {
            Vector3 finalPos = player.position;
            finalPos.y = transform.position.y;
            transform.LookAt(finalPos);

            moveDir = new Vector3(0, moveDir.y, speed);
            moveDir = transform.TransformDirection(moveDir);
            control.MovePosition(control.position + moveDir * Time.deltaTime);
        }
        else
        {
            playerOnArea = false;
        }
    }

    void DetectPlayer()
    {
        if (detectCounterOn) detectCounter -= Time.deltaTime;
        if (detectCounter <= 0)
        {
            detectCounter = 2;
            detectCounterOn = false;
            speed = 1.5f;
        }
        float dist = Vector3.Distance(transform.position, center.position);
        if (dist < followDistance && detectCounter == 2)
        {
            playerDetect = Physics.OverlapBox(transform.position, new Vector3(5, 2.25f, 5), Quaternion.identity, playerMask).Length > 0;
            if (playerDetect)
            {
                playerOnArea = true;
                speed = 3;
            }
            else
            {
                playerOnArea = false;
                speed = 1.5f;
            }
        }
        else
        {
            detectCounterOn = true;
        }
    }

    void WalkAnims()
    {
        if (speed == 1.5f)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
        }
        else if (speed == 3)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Walk", false);
        }
    }

    public void AttackAnim()
    {
        anim.SetTrigger("Attack");
    }

    public void DeathAnim()
    {
        anim.SetTrigger("Death");
    }
}
