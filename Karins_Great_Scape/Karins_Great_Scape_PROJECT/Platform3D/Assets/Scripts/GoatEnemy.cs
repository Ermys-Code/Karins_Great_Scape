using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatEnemy : MonoBehaviour
{
    Rigidbody control;
    public bool isGrounded, playerOnArea, playerDetect;
    RaycastHit hitWalls;
    Vector3 moveDir;
    float speed = 3, jumpSpeed = 10;
    int currentPathPoint;
    float detectCounter = 2;
    bool detectCounterOn;

    public Transform player, center;
    public LayerMask groundMask, playerMask;
    public List<Transform> pathPoints;
    public bool isJumping, haveHelmet;
    public float followDistance;

    void Start()
    {
        control = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
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
    }

    void Move()
    {
        
        Vector3 finalPos = pathPoints[currentPathPoint].position;
        finalPos.y = transform.position.y;
        transform.LookAt(finalPos);

        moveDir = new Vector3(0, moveDir.y, speed);
        moveDir = transform.TransformDirection(moveDir);
        control.MovePosition(control.position + moveDir * Time.deltaTime);

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

        if (isGrounded && !isJumping && Physics.Raycast(rayPos, transform.forward, out hitWalls, 2, groundMask))
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
        }
        float dist = Vector3.Distance(transform.position, center.position);
        if (dist < followDistance && detectCounter == 2)
        {
            playerDetect = Physics.OverlapBox(transform.position, new Vector3(5, 2.25f, 5), Quaternion.identity, playerMask).Length > 0;
            if (playerDetect)
            {
                playerOnArea = true;
            }
            else
            {
                playerOnArea = false;
            }
        }
        else
        {
            detectCounterOn = true;
        }
    }
}
