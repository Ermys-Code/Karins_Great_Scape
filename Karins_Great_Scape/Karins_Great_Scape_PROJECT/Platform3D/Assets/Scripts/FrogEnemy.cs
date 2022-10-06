using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEnemy : MonoBehaviour
{
    Rigidbody control;
    Vector3 moveDir;
    bool isGrounded, landed;
    float jumpCount, speed = 3, jumpSpeed = 8;
    int currentPathPoint;
    float landTimer;
    bool landing;

    public LayerMask groundMask;
    public List<Transform> pathPoints;
    public bool isJumping;
    public Animator anim;

    void Start()
    {
        control = GetComponent<Rigidbody>();

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        isGrounded = Physics.OverlapSphere(transform.position, 0.2f, groundMask).Length > 0;

        jumpCount += Time.deltaTime;

        if (isGrounded && !isJumping && jumpCount > 2)
        {
            float dist = Vector3.Distance(transform.position, pathPoints[currentPathPoint].position);
            if (dist <= 2)
            {
                currentPathPoint++;
                if (currentPathPoint >= pathPoints.Count) currentPathPoint = 0;
            }

            Vector3 finalPos = pathPoints[currentPathPoint].position;
            finalPos.y = transform.position.y;
            transform.LookAt(finalPos);

            jumpCount = 0;
            control.velocity = new Vector3(control.velocity.x, 0, control.velocity.z);
            control.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            landed = false;
            isJumping = true;
            
        }

        if (isJumping && control.velocity.y < 0) isJumping = false;

        if (!isGrounded)
        {
            moveDir = new Vector3(0, moveDir.y, speed);
            moveDir = transform.TransformDirection(moveDir);
            control.MovePosition(control.position + moveDir * Time.deltaTime);
        }

        if(isJumping) anim.SetFloat("AnimationSime", 2, 5, 1);
        else if(!isJumping && !isGrounded) anim.SetFloat("AnimationSime", 1, 5, 1);
        if (isGrounded && !landed)
        {
            anim.SetFloat("AnimationSime", 0, 5, 1);


            landing = true;
            if (landing)
            {
                landTimer += Time.deltaTime;
                if(landTimer >= 0.2f)
                {
                    landed = true;
                    landTimer = 0;
                }
            }
        }
        else if (isGrounded && landed)
        {
            anim.SetFloat("AnimationSime", 1, 5, 1);
        }
            


    }
}
